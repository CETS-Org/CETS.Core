using Application.Interfaces.ACAD;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using Domain.Interfaces.CORE;
using Domain.Interfaces.FIN;
using Domain.Interfaces.IDN;
using Domain.Settings;
using DTOs.ACAD.ACAD_AcademicRequest.Requests;
using DTOs.ACAD.ACAD_AcademicRequest.Responses;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.ACAD
{
    public class ACAD_SuspensionValidationService : IACAD_SuspensionValidationService
    {
        private readonly IACAD_AcademicRequestRepository _requestRepo;
        private readonly IIDN_AccountRepository _accountRepo;
        private readonly IFIN_InvoiceRepository _invoiceRepo;
        private readonly ICORE_LookUpRepository _lookUpRepo;
        private readonly SuspensionPolicySettings _policy;

        public ACAD_SuspensionValidationService(
            IACAD_AcademicRequestRepository requestRepo,
            IIDN_AccountRepository accountRepo,
            IFIN_InvoiceRepository invoiceRepo,
            ICORE_LookUpRepository lookUpRepo,
            IOptions<SuspensionPolicySettings> policy)
        {
            _requestRepo = requestRepo;
            _accountRepo = accountRepo;
            _invoiceRepo = invoiceRepo;
            _lookUpRepo = lookUpRepo;
            _policy = policy.Value;
        }

        public async Task<SuspensionValidationResult> ValidateSuspensionRequestAsync(CreateSuspensionRequest request)
        {
            var result = new SuspensionValidationResult();

            // Validate dates
            await ValidateDatesAsync(request.StartDate, request.EndDate, result);

            // Validate reason category
            ValidateReasonCategory(request.ReasonCategory, request.ReasonDetail, result);

            // Validate student eligibility
            await ValidateStudentEligibilityAsync(request.StudentID, result);

            // Validate document requirements
            ValidateDocumentRequirements(request.StartDate, request.EndDate, request.AttachmentUrl, result);

            result.IsValid = result.Errors.Count == 0;
            return result;
        }

        public async Task<SuspensionValidationResult> ValidateSuspensionRequestAsync(CreateAcademicRequest request)
        {
            if (!request.SuspensionStartDate.HasValue || !request.SuspensionEndDate.HasValue)
            {
                return new SuspensionValidationResult
                {
                    IsValid = false,
                    Errors = new List<string> { "Suspension start date and end date are required." }
                };
            }

            var result = new SuspensionValidationResult();

            // Validate dates
            await ValidateDatesAsync(request.SuspensionStartDate.Value, request.SuspensionEndDate.Value, result);

            // Validate reason category
            if (!string.IsNullOrEmpty(request.ReasonCategory))
            {
                ValidateReasonCategory(request.ReasonCategory, request.Reason, result);
            }

            // Validate student eligibility
            await ValidateStudentEligibilityAsync(request.StudentID, result);

            // Validate document requirements
            ValidateDocumentRequirements(request.SuspensionStartDate.Value, request.SuspensionEndDate.Value, request.AttachmentUrl, result);

            result.IsValid = result.Errors.Count == 0;
            return result;
        }

        private async Task ValidateDatesAsync(DateOnly startDate, DateOnly endDate, SuspensionValidationResult result)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            // Rule: StartDate >= Today + NoticePeriodDays
            var minStartDate = today.AddDays(_policy.NoticePeriodDays);
            if (startDate < minStartDate)
            {
                result.Errors.Add($"Suspension must be submitted at least {_policy.NoticePeriodDays} days before the start date. Earliest start date: {minStartDate:yyyy-MM-dd}");
            }

            // Rule: EndDate > StartDate
            if (endDate <= startDate)
            {
                result.Errors.Add("End date must be after start date.");
            }

            // Calculate duration
            result.DurationDays = endDate.DayNumber - startDate.DayNumber;

            // Rule: Duration >= MinDays
            if (result.DurationDays < _policy.MinDays)
            {
                result.Errors.Add($"Suspension duration must be at least {_policy.MinDays} days. Current duration: {result.DurationDays} days.");
            }

            // Rule: Duration <= MaxDays
            if (result.DurationDays > _policy.MaxDays)
            {
                result.Errors.Add($"Suspension duration cannot exceed {_policy.MaxDays} days. Current duration: {result.DurationDays} days.");
            }
        }

        private void ValidateReasonCategory(string category, string detail, SuspensionValidationResult result)
        {
            // Validate category is in allowed list
            if (!SuspensionReasonCategories.All.Contains(category))
            {
                result.Errors.Add($"Invalid reason category. Allowed values: {string.Join(", ", SuspensionReasonCategories.All)}");
            }

            // Validate that certain categories require detailed explanation
            if (string.IsNullOrWhiteSpace(detail) && (
                category == SuspensionReasonCategories.Other ||
                category == SuspensionReasonCategories.FinancialDifficulty ||
                category == SuspensionReasonCategories.FamilyIssue))
            {
                result.Errors.Add($"Detailed explanation is required for reason category: {category}");
            }

            // Add warnings for insufficient detail
            if (!string.IsNullOrWhiteSpace(detail) && detail.Length < 20)
            {
                result.Warnings.Add("Please provide more detailed explanation for your suspension request.");
            }
        }

        private async Task ValidateStudentEligibilityAsync(Guid studentId, SuspensionValidationResult result)
        {
            // Get student account
            var account = await _accountRepo.GetDetailByIdAsync(studentId);
            if (account == null)
            {
                result.Errors.Add("Student account not found.");
                return;
            }

            // Check if student is active
            var activeStatus = await _lookUpRepo.GetByCodeAsync(LookUpTypes.AccountStatus, "Active");
            if (activeStatus != null && account.AccountStatusID != activeStatus.Id)
            {
                result.Errors.Add("Only active students can submit suspension requests.");
            }

            // Check for unpaid tuition
            var unpaidInvoices = await _invoiceRepo.GetUnpaidInvoicesByStudentAsync(studentId);
            if (unpaidInvoices.Any())
            {
                result.Errors.Add("Student has unpaid tuition. Please clear outstanding payments before submitting a suspension request.");
            }

            // Check suspension count this year
            var currentYear = DateTime.Now.Year;
            var yearStart = new DateTime(currentYear, 1, 1);
            
            var suspensionRequestType = await _lookUpRepo.GetByCodeAsync(LookUpTypes.AcademicRequestType, "Suspension");
            if (suspensionRequestType == null)
            {
                result.Warnings.Add("Unable to verify suspension count - request type not found in system.");
                return;
            }

            var approvedStatus = await _lookUpRepo.GetByCodeAsync(LookUpTypes.AcademicRequestStatus, "Approved");
            var suspendedStatus = await _lookUpRepo.GetByCodeAsync(LookUpTypes.AcademicRequestStatus, "Suspended");
            var completedStatus = await _lookUpRepo.GetByCodeAsync(LookUpTypes.AcademicRequestStatus, "Completed");

            var existingRequests = await _requestRepo.GetByStudentAsync(studentId);
            var suspensionCountThisYear = existingRequests.Count(r =>
                r.RequestTypeID == suspensionRequestType.Id &&
                r.CreatedAt >= yearStart &&
                (approvedStatus != null && r.AcademicRequestStatusID == approvedStatus.Id ||
                 suspendedStatus != null && r.AcademicRequestStatusID == suspendedStatus.Id ||
                 completedStatus != null && r.AcademicRequestStatusID == completedStatus.Id));

            result.SuspensionCountThisYear = suspensionCountThisYear;

            if (suspensionCountThisYear >= _policy.MaxSuspensionsPerYear)
            {
                result.Errors.Add($"Student has already requested {suspensionCountThisYear} suspension(s) this year. Maximum allowed: {_policy.MaxSuspensionsPerYear}");
            }

            // Check for already suspended status
            var suspendedAccountStatus = await _lookUpRepo.GetByCodeAsync(LookUpTypes.AccountStatus, "Suspended");
            if (suspendedAccountStatus != null && account.AccountStatusID == suspendedAccountStatus.Id)
            {
                result.Errors.Add("Student is already suspended.");
            }

            // Check for overlapping suspension periods
            var pendingStatus = await _lookUpRepo.GetByCodeAsync(LookUpTypes.AcademicRequestStatus, "Pending");
            var underReviewStatus = await _lookUpRepo.GetByCodeAsync(LookUpTypes.AcademicRequestStatus, "UnderReview");
            
            var activeSuspensions = existingRequests.Where(r =>
                r.RequestTypeID == suspensionRequestType.Id &&
                r.SuspensionStartDate.HasValue &&
                r.SuspensionEndDate.HasValue &&
                (pendingStatus != null && r.AcademicRequestStatusID == pendingStatus.Id ||
                 underReviewStatus != null && r.AcademicRequestStatusID == underReviewStatus.Id ||
                 approvedStatus != null && r.AcademicRequestStatusID == approvedStatus.Id ||
                 suspendedStatus != null && r.AcademicRequestStatusID == suspendedStatus.Id));

            if (activeSuspensions.Any())
            {
                result.Errors.Add("Student already has an active or pending suspension request.");
            }
        }

        private void ValidateDocumentRequirements(DateOnly startDate, DateOnly endDate, string? documentUrl, SuspensionValidationResult result)
        {
            var duration = endDate.DayNumber - startDate.DayNumber;
            
            result.RequiresDocument = duration >= _policy.RequireDocumentOverDays;

            if (result.RequiresDocument && string.IsNullOrWhiteSpace(documentUrl))
            {
                result.Errors.Add($"Supporting document is required for suspensions longer than {_policy.RequireDocumentOverDays} days.");
            }
            else if (duration >= 14 && string.IsNullOrWhiteSpace(documentUrl))
            {
                result.Warnings.Add("Supporting document is recommended for suspensions of 14 days or longer.");
            }
        }

    }
}

