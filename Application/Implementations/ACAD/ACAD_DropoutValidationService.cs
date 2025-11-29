using Application.Interfaces.ACAD;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using Domain.Interfaces.CORE;
using Domain.Interfaces.FIN;
using Domain.Interfaces.IDN;
using DTOs.ACAD.ACAD_AcademicRequest.Requests;
using DTOs.ACAD.ACAD_AcademicRequest.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.ACAD
{
    public class ACAD_DropoutValidationService : IACAD_DropoutValidationService
    {
        private readonly IACAD_AcademicRequestRepository _requestRepo;
        private readonly IIDN_AccountRepository _accountRepo;
        private readonly IFIN_InvoiceRepository _invoiceRepo;
        private readonly ICORE_LookUpRepository _lookUpRepo;

        public ACAD_DropoutValidationService(
            IACAD_AcademicRequestRepository requestRepo,
            IIDN_AccountRepository accountRepo,
            IFIN_InvoiceRepository invoiceRepo,
            ICORE_LookUpRepository lookUpRepo)
        {
            _requestRepo = requestRepo;
            _accountRepo = accountRepo;
            _invoiceRepo = invoiceRepo;
            _lookUpRepo = lookUpRepo;
        }

        public async Task<DropoutValidationResult> ValidateDropoutRequestAsync(CreateDropoutRequest request)
        {
            var result = new DropoutValidationResult
            {
                CompletedExitSurvey = request.CompletedExitSurvey
            };

            // Validate effective date
            ValidateEffectiveDate(request.EffectiveDate, result);

            // Validate reason category
            ValidateReasonCategory(request.ReasonCategory, request.ReasonDetail, result);

            // Validate student eligibility
            await ValidateStudentEligibilityAsync(request.StudentID, result);

            // Validate exit survey completion
            ValidateExitSurvey(request.CompletedExitSurvey, request.ExitSurveyUrl, result);

            result.IsValid = result.Errors.Count == 0;
            return result;
        }

        public async Task<DropoutValidationResult> ValidateDropoutRequestAsync(CreateAcademicRequest request)
        {
            var result = new DropoutValidationResult
            {
                CompletedExitSurvey = request.CompletedExitSurvey ?? false
            };

            // Validate effective date if provided (will be set to default 7 days if not provided)
            if (request.EffectiveDate.HasValue)
            {
                ValidateEffectiveDate(request.EffectiveDate.Value, result);
            }

            // Validate reason category
            if (!string.IsNullOrEmpty(request.ReasonCategory))
            {
                ValidateReasonCategory(request.ReasonCategory, request.Reason, result);
            }

            // Validate student eligibility
            await ValidateStudentEligibilityAsync(request.StudentID, result);

            // Validate exit survey completion
            ValidateExitSurvey(request.CompletedExitSurvey ?? false, request.ExitSurveyUrl, result);

            result.IsValid = result.Errors.Count == 0;
            return result;
        }

        private void ValidateEffectiveDate(DateOnly effectiveDate, DropoutValidationResult result)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            // Rule: EffectiveDate >= Today (cannot be a past date)
            if (effectiveDate < today)
            {
                result.Errors.Add("Effective date cannot be in the past. It must be today or a future date.");
            }
        }

        private void ValidateReasonCategory(string category, string detail, DropoutValidationResult result)
        {
            // Validate category is in allowed list
            if (!DropoutReasonCategories.All.Contains(category))
            {
                result.Errors.Add($"Invalid reason category. Allowed values: {string.Join(", ", DropoutReasonCategories.All)}");
            }

            // Validate that reason detail is not empty
            if (string.IsNullOrWhiteSpace(detail))
            {
                result.Errors.Add("Detailed explanation is required for dropout request.");
            }

            // Add warnings for insufficient detail
            if (!string.IsNullOrWhiteSpace(detail) && detail.Length < 20)
            {
                result.Warnings.Add("Please provide more detailed explanation for your dropout request.");
            }

            // Check for specific categories that need more explanation
            if (category == DropoutReasonCategories.UnsatisfiedWithCourse ||
                category == DropoutReasonCategories.UnsatisfiedWithTeacher)
            {
                if (!string.IsNullOrWhiteSpace(detail) && detail.Length < 50)
                {
                    result.Warnings.Add("Please provide a detailed explanation when reporting dissatisfaction to help us improve our services.");
                }
            }
        }

        private async Task ValidateStudentEligibilityAsync(Guid studentId, DropoutValidationResult result)
        {
            // Get student account
            var account = await _accountRepo.GetDetailByIdAsync(studentId);
            if (account == null)
            {
                result.Errors.Add("Student account not found.");
                return;
            }

            // Check if student is in a valid status to drop out
            var activeStatus = await _lookUpRepo.GetByCodeAsync(LookUpTypes.AccountStatus, "Active");
            var awaitingReturnStatus = await _lookUpRepo.GetByCodeAsync(LookUpTypes.AccountStatus, "AwaitingReturn");
            
            bool isEligible = false;
            if (activeStatus != null && account.AccountStatusID == activeStatus.Id)
            {
                isEligible = true;
            }
            if (awaitingReturnStatus != null && account.AccountStatusID == awaitingReturnStatus.Id)
            {
                isEligible = true;
            }

            if (!isEligible)
            {
                // Check if already in terminal states
                var droppedOutStatus = await _lookUpRepo.GetByCodeAsync(LookUpTypes.AccountStatus, "DroppedOut");
                var completedStatus = await _lookUpRepo.GetByCodeAsync(LookUpTypes.AccountStatus, "Completed");
                var expelledStatus = await _lookUpRepo.GetByCodeAsync(LookUpTypes.AccountStatus, "Expelled");

                if (droppedOutStatus != null && account.AccountStatusID == droppedOutStatus.Id)
                {
                    result.Errors.Add("Student has already dropped out.");
                }
                else if (completedStatus != null && account.AccountStatusID == completedStatus.Id)
                {
                    result.Errors.Add("Student has already completed the program.");
                }
                else if (expelledStatus != null && account.AccountStatusID == expelledStatus.Id)
                {
                    result.Errors.Add("Student has been expelled.");
                }
                else
                {
                    result.Errors.Add("Only active or awaiting return students can submit dropout requests.");
                }
            }

            // Check for unpaid tuition
            var unpaidInvoices = await _invoiceRepo.GetUnpaidInvoicesByStudentAsync(studentId);
            if (unpaidInvoices.Any())
            {
                result.HasUnpaidInvoices = true;
                result.Errors.Add("Student has unpaid tuition. All financial obligations must be settled before dropping out.");
            }

            // Check for other pending academic requests
            var existingRequests = await _requestRepo.GetByStudentAsync(studentId);
            var pendingStatus = await _lookUpRepo.GetByCodeAsync(LookUpTypes.AcademicRequestStatus, "Pending");
            var underReviewStatus = await _lookUpRepo.GetByCodeAsync(LookUpTypes.AcademicRequestStatus, "UnderReview");
            var needInfoStatus = await _lookUpRepo.GetByCodeAsync(LookUpTypes.AcademicRequestStatus, "NeedInfo");

            var pendingRequests = existingRequests.Where(r =>
                (pendingStatus != null && r.AcademicRequestStatusID == pendingStatus.Id) ||
                (underReviewStatus != null && r.AcademicRequestStatusID == underReviewStatus.Id) ||
                (needInfoStatus != null && r.AcademicRequestStatusID == needInfoStatus.Id));

            if (pendingRequests.Any())
            {
                result.HasPendingRequests = true;
                result.Errors.Add("Student has other pending academic requests. Please close or complete them before submitting a dropout request.");
            }

            // Check if already in suspension
            var suspendedStatus = await _lookUpRepo.GetByCodeAsync(LookUpTypes.AccountStatus, "Suspended");
            if (suspendedStatus != null && account.AccountStatusID == suspendedStatus.Id)
            {
                result.Warnings.Add("Student is currently suspended. Dropping out while suspended typically does not qualify for refunds.");
            }

            // Check for existing dropout request
            var dropoutRequestType = await _lookUpRepo.GetByCodeAsync(LookUpTypes.AcademicRequestType, "Dropout");
            if (dropoutRequestType != null)
            {
                var existingDropoutRequest = existingRequests.FirstOrDefault(r =>
                    r.RequestTypeID == dropoutRequestType.Id &&
                    ((pendingStatus != null && r.AcademicRequestStatusID == pendingStatus.Id) ||
                     (underReviewStatus != null && r.AcademicRequestStatusID == underReviewStatus.Id) ||
                     (needInfoStatus != null && r.AcademicRequestStatusID == needInfoStatus.Id)));

                if (existingDropoutRequest != null)
                {
                    result.Errors.Add("Student already has a pending dropout request.");
                }
            }
        }

        private void ValidateExitSurvey(bool completedExitSurvey, string? exitSurveyUrl, DropoutValidationResult result)
        {
            if (!completedExitSurvey)
            {
                result.Errors.Add("Exit survey must be completed before submitting a dropout request.");
            }

            if (completedExitSurvey && string.IsNullOrWhiteSpace(exitSurveyUrl))
            {
                result.Errors.Add("Exit survey URL is required when survey is marked as completed.");
            }

            if (!string.IsNullOrWhiteSpace(exitSurveyUrl) && !completedExitSurvey)
            {
                result.Warnings.Add("Exit survey URL is provided but survey is not marked as completed.");
            }
        }
    }
}

