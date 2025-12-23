using Application.Interfaces.ACAD;
using Application.Interfaces.Common.Storage;
using Application.Interfaces.COM;
using Application.Interfaces.Common.Email;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using Domain.Interfaces.CORE;
using Domain.Interfaces.IDN;
using DTOs.ACAD.ACAD_AcademicRequest.Requests;
using DTOs.ACAD.ACAD_AcademicRequest.Responses;
using DTOs.ACAD.ACAD_AcademicRequestHistory.Responses;
using DTOs.COM.COM_Notification.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Implementations.ACAD
{
    public class ACAD_AcademicRequestService : IACAD_AcademicRequestService
    {
        private readonly IACAD_AcademicRequestRepository _requestRepo;
        private readonly IACAD_AcademicRequestHistoryRepository _historyRepo;
        private readonly IACAD_EnrollmentRepository _enrollmentRepo;
        private readonly IACAD_ClassReservationRepository _classReservationRepo;
        private readonly ICORE_LookUpRepository _lookUpRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IACAD_ClassMeetingRepository _classMeetingRepo;
        private readonly IIDN_AccountRepository _accountRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICOM_NotificationService _notificationService;
        private readonly IACAD_SuspensionValidationService? _suspensionValidationService;
        private readonly IACAD_DropoutValidationService? _dropoutValidationService;
        private readonly IMailService _mailService;
        private readonly IEmailTemplateBuilder _emailTemplateBuilder;

        public ACAD_AcademicRequestService(
            IACAD_AcademicRequestRepository requestRepo,
            IACAD_AcademicRequestHistoryRepository historyRepo,
            IACAD_EnrollmentRepository enrollmentRepo,
            IACAD_ClassReservationRepository classReservationRepo,
            ICORE_LookUpRepository lookUpRepository,
            IFileStorageService fileStorageService,
            IACAD_ClassMeetingRepository classMeetingRepo,
            IIDN_AccountRepository accountRepo,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICOM_NotificationService notificationService,
            IMailService mailService,
            IEmailTemplateBuilder emailTemplateBuilder,
            IACAD_SuspensionValidationService? suspensionValidationService = null,
            IACAD_DropoutValidationService? dropoutValidationService = null)
        {
            _requestRepo = requestRepo;
            _historyRepo = historyRepo;
            _enrollmentRepo = enrollmentRepo;
            _classReservationRepo = classReservationRepo;
            _lookUpRepository = lookUpRepository;
            _fileStorageService = fileStorageService;
            _classMeetingRepo = classMeetingRepo;
            _accountRepo = accountRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
            _mailService = mailService;
            _emailTemplateBuilder = emailTemplateBuilder;
            _suspensionValidationService = suspensionValidationService;
            _dropoutValidationService = dropoutValidationService;
        }

        public async Task<AcademicRequestResponse> SubmitRequestAsync(CreateAcademicRequest requestDto)
        {
           
            // Get request type to check if it's a suspension request
            var requestType = await _lookUpRepository.GetByIdAsync(requestDto.RequestTypeID);
            if (requestType == null)
            {
                throw new KeyNotFoundException("Request type not found. Please ensure the lookup data is properly seeded.");
            }

            var requestTypeCode = (requestType.Code ?? "").ToLower();
            var isSuspension = requestTypeCode.Contains("suspension");
            var isDropout =  requestTypeCode.Contains("dropout");
            var isRefund = requestTypeCode.Contains("refund");
            var isTransfer = requestTypeCode.Contains("transfer");

            // Validate suspension requests
            if (isSuspension && _suspensionValidationService != null)
            {
                var validationResult = await _suspensionValidationService.ValidateSuspensionRequestAsync(requestDto);
                if (!validationResult.IsValid)
                {
                    throw new InvalidOperationException($"Suspension request validation failed: {string.Join("; ", validationResult.Errors)}");
                }
            }

            // Validate dropout requests
            if (isDropout && _dropoutValidationService != null)
            {
                var validationResult = await _dropoutValidationService.ValidateDropoutRequestAsync(requestDto);
                if (!validationResult.IsValid)
                {
                    throw new InvalidOperationException($"Dropout request validation failed: {string.Join("; ", validationResult.Errors)}");
                }
            }

            //Validate transfer requests
            if (isTransfer)
            {
                if (!requestDto.EnrollmentID.HasValue)
                {
                    if (!requestDto.FromClassID.HasValue)
                        throw new InvalidOperationException(
                            "EnrollmentID or FromClassID is required for class transfer.");

                    var enrollment = await _enrollmentRepo
                        .GetByStudentAndClassAsync(requestDto.StudentID, requestDto.FromClassID.Value);

                    if (enrollment == null)
                        throw new KeyNotFoundException(
                            "Enrollment not found for the given student and class.");

                    requestDto.EnrollmentID = enrollment.Id;
                }

                var enrollmentStatus = await _lookUpRepository.GetByIdAsync(
                    (await _enrollmentRepo.GetByIdAsync(requestDto.EnrollmentID.Value))!.EnrollmentStatusID
                );

                if ((enrollmentStatus?.Code ?? "").ToLower() != "enrolled")
                    throw new InvalidOperationException("Only enrolled students can transfer class.");
            }

            // Validate EnrollmentID exists if provided
            var isEnrollmentCancellation = requestTypeCode.Contains("cancel");
            var isReturnFromSuspension = requestTypeCode.Contains("resume");
            
            if (requestDto.EnrollmentID.HasValue)
            {
                var enrollment = await _enrollmentRepo.GetByIdAsync(requestDto.EnrollmentID.Value);
                if (enrollment == null)
                {
                    throw new KeyNotFoundException($"Enrollment with ID {requestDto.EnrollmentID.Value} not found. Please ensure you have selected a valid enrollment.");
                }

                // For cancellation, verify enrollment is in Pending status
                if (isEnrollmentCancellation)
                {
                    var enrollmentStatus = await _lookUpRepository.GetByIdAsync(enrollment.EnrollmentStatusID);
                    var statusCode = (enrollmentStatus?.Code ?? "").ToLower();
                    if (statusCode != "pending" && statusCode != "pendingconfirmation")
                    {
                        throw new InvalidOperationException($"Only pending enrollments can be cancelled. This enrollment status is: {enrollmentStatus?.Name ?? "Unknown"}");
                    }
                }

                // For suspension/dropout, verify enrollment is in Enrolled status
                if (isSuspension || isDropout)
                {
                    var enrollmentStatus = await _lookUpRepository.GetByIdAsync(enrollment.EnrollmentStatusID);
                    var statusCode = (enrollmentStatus?.Code ?? "").ToLower();
                    if (statusCode != "enrolled")
                    {
                        throw new InvalidOperationException($"Only enrolled courses can be {(isSuspension ? "suspended" : "dropped out from")}. This enrollment status is: {enrollmentStatus?.Name ?? "Unknown"}");
                    }
                }

                // For return from suspension, verify enrollment is Suspended or AwaitingReturn
                if (isReturnFromSuspension)
                {
                    var enrollmentStatus = await _lookUpRepository.GetByIdAsync(enrollment.EnrollmentStatusID);
                    var statusCode = (enrollmentStatus?.Code ?? "").ToLower();
                    if (statusCode != "suspended" && statusCode != "awaitingreturn")
                    {
                        throw new InvalidOperationException($"Only suspended or awaiting return enrollments can be reactivated. This enrollment status is: {enrollmentStatus?.Name ?? "Unknown"}");
                    }
                }
            }
            else if (isSuspension || isDropout || isEnrollmentCancellation || isReturnFromSuspension || isRefund)
            {
                // EnrollmentID is required for these request types
                throw new InvalidOperationException($"EnrollmentID is required for {(isSuspension ? "suspension" : isDropout ? "dropout" : "cancellation")} requests.");
            }

            var entity = _mapper.Map<ACAD_AcademicRequest>(requestDto);
            
            var pendingStatus = await _lookUpRepository.GetByCodeAsync(LookUpTypes.AcademicRequestStatus, "Pending");
            if (pendingStatus == null)
            {
                throw new KeyNotFoundException("Pending status not found for AcademicRequestStatus. Please ensure the lookup data is properly seeded.");
            }
            
            entity.AcademicRequestStatusID = pendingStatus.Id;

            // Set default priority based on request type if not provided
            if (!requestDto.PriorityID.HasValue || requestDto.PriorityID.Value == Guid.Empty)
            {
                string priorityCode = "Medium";

                if (requestTypeCode.Contains("reschedule") || requestTypeCode.Contains("transfer"))
                {
                    priorityCode = "High";
                }
                else if (requestTypeCode.Contains("cancel") || requestTypeCode.Contains("suspension") || requestTypeCode.Contains("dropout"))
                {
                    priorityCode = "Medium";
                }
                else if (requestTypeCode.Contains("other"))
                {
                    priorityCode = "Low";
                }

                var defaultPriority = await _lookUpRepository.GetByCodeAsync(LookUpTypes.Priority, priorityCode);
                if (defaultPriority == null)
                {
                    throw new KeyNotFoundException($"Default priority ({priorityCode}) not found for Priority. Please ensure the lookup data is properly seeded.");
                }
                entity.PriorityID = defaultPriority.Id;
            }

            // Set EffectiveDate based on request type:
            // - 3 days for meeting reschedule (if not provided)
            // - Use SuspensionStartDate for suspension
            // - Use provided EffectiveDate for dropout (or default to 7 days if not provided)
            // - 7 days for all other requests (if not provided)
            if (requestTypeCode.Contains("reschedule"))
            {
                entity.EffectiveDate = requestDto.EffectiveDate ?? DateOnly.FromDateTime(DateTime.Now.AddDays(3));
            }
            else if (isSuspension && requestDto.SuspensionStartDate.HasValue)
            {
                entity.EffectiveDate = requestDto.SuspensionStartDate.Value;
            }
            else if (isDropout)
            {
                // For dropout, use provided EffectiveDate from frontend (required for dropout)
                // Only default to 7 days if not provided (for backwards compatibility)
                entity.EffectiveDate = requestDto.EffectiveDate ?? DateOnly.FromDateTime(DateTime.Now.AddDays(7));
            }
            else
            {
                entity.EffectiveDate = requestDto.EffectiveDate ?? DateOnly.FromDateTime(DateTime.Now.AddDays(7));
            }

            // For suspension requests, set ExpectedReturnDate
            if (isSuspension && requestDto.SuspensionEndDate.HasValue)
            {
                entity.ExpectedReturnDate = requestDto.SuspensionEndDate.Value.AddDays(1);
            }

            _requestRepo.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            // Create history entry for request submission
            var history = new ACAD_AcademicRequestHistory
            {
                RequestID = entity.Id,
                StatusID = pendingStatus.Id,
                AttachmentUrl = requestDto.AttachmentUrl
            };

            _historyRepo.Add(history);
            await _unitOfWork.SaveChangesAsync();

            // Send email notification for dropout request submission
            if (isDropout)
            {
                await SendDropoutRequestSubmittedEmailAsync(entity, requestType);
            }

            return _mapper.Map<AcademicRequestResponse>(entity);
        }

        public async Task ProcessRequestAsync(ProcessAcademicRequest requestDto)
        {
            var entity = await _requestRepo.GetDetailsAsync(requestDto.RequestID);
            if (entity == null)
                throw new KeyNotFoundException("Request not found");

            // Validate that the status exists
            var status = await _lookUpRepository.GetByIdAsync(requestDto.StatusID);
            if (status == null)
            {
                throw new KeyNotFoundException($"Status with ID {requestDto.StatusID} not found. Please ensure the lookup data is properly seeded.");
            }

            // If this is an approved meeting reschedule request, handle the reschedule
            var approvedStatus = await _lookUpRepository.GetByCodeAsync(LookUpTypes.AcademicRequestStatus, "Approved");
            if (approvedStatus != null && requestDto.StatusID == approvedStatus.Id && entity.ClassMeetingID.HasValue)
            {
                // Use staff-selected room if provided, otherwise use the room from the request
                if (requestDto.SelectedRoomID.HasValue)
                {
                    entity.NewRoomID = requestDto.SelectedRoomID.Value;
                }
                await HandleMeetingRescheduleAsync(entity);
            }

            // If this is an approved suspension request, handle suspension
            var requestType = await _lookUpRepository.GetByIdAsync(entity.RequestTypeID);
            var requestTypeCode = (requestType?.Code ?? "").ToLower();
            var isSuspension = requestTypeCode.Contains("suspension");
            var isDropout = requestTypeCode.Contains("dropout");
            var isEnrollmentCancellation = requestTypeCode.Contains("cancel");
            var isRefund = requestTypeCode.Contains("refund");
            var isTransfer = requestTypeCode.Contains("transfer");

            if (approvedStatus != null && requestDto.StatusID == approvedStatus.Id && isSuspension)
            {   
                await HandleSuspensionApprovalAsync(entity);
            }

            // If this is an approved enrollment cancellation request, handle cancellation
            if (approvedStatus != null && requestDto.StatusID == approvedStatus.Id && isEnrollmentCancellation)
            {
                await HandleEnrollmentCancellationAsync(entity);
            }

            // If this is an approved refund request, handle refund enrollment status
            if (approvedStatus != null && requestDto.StatusID == approvedStatus.Id && isRefund)
            {
                await HandleRefundApprovalAsync(entity);
            }

            // If this is an approved return from suspension request, handle return
            var isReturnFromSuspension = requestTypeCode.Contains("resume");
            
            if (approvedStatus != null && requestDto.StatusID == approvedStatus.Id && isReturnFromSuspension)
            {
                await HandleReturnFromSuspensionAsync(entity);
            }

            // If this is a completed dropout request, handle dropout finalization
            var completedStatus = await _lookUpRepository.GetByCodeAsync(LookUpTypes.AcademicRequestStatus, "Completed");
            if (completedStatus != null && requestDto.StatusID == completedStatus.Id && isDropout)
            {
                await HandleDropoutCompletionAsync(entity);
            }

            if (approvedStatus != null && requestDto.StatusID == approvedStatus.Id && isTransfer)
            {
                await HandleClassTransferAsync(entity);
            }

            entity.AcademicRequestStatusID = requestDto.StatusID;
            entity.ProcessedBy = requestDto.StaffID;
            entity.StaffResponse = requestDto.Description; 
            _requestRepo.Update(entity);

            var history = _mapper.Map<ACAD_AcademicRequestHistory>(requestDto);
            history.RequestID = requestDto.RequestID;
            history.StatusID = requestDto.StatusID; 

            _historyRepo.Add(history);
            await _unitOfWork.SaveChangesAsync();

            // Send notification to the student about the request status change
            await SendRequestStatusNotificationAsync(entity, status);
            
            // Send email notification for dropout request status change
            if (isDropout && (requestDto.StatusID == approvedStatus?.Id || requestDto.StatusID == completedStatus?.Id))
            {
                await SendDropoutRequestStatusEmailAsync(entity, status);
            }
        }

        private async Task HandleMeetingRescheduleAsync(ACAD_AcademicRequest request)
        {
            if (!request.ClassMeetingID.HasValue || !request.ToMeetingDate.HasValue)
                return;

            var meeting = await _classMeetingRepo.GetByIdAsync(request.ClassMeetingID.Value);
            if (meeting == null)
                throw new KeyNotFoundException("Class meeting not found");

            var originalDate = meeting.Date;
            var originalCoveredTopicId = meeting.CoveredTopicID;

            // Get all meetings for this class that are on or after the original date
            var allClassMeetings = await _classMeetingRepo.GetAllClassMeetingByClassId(meeting.ClassID);
            var futureMeetings = allClassMeetings
                .Where(m => m.Date >= originalDate && !m.IsDeleted)
                .OrderBy(m => m.Date)
                .ToList();

            // Update the rescheduled meeting's date, slot, and room
            // Using ToMeetingDate and ToSlotID for the new meeting details
            meeting.Date = request.ToMeetingDate.Value;
            if (request.ToSlotID.HasValue)
                meeting.SlotID = request.ToSlotID.Value;
            if (request.NewRoomID.HasValue)
                meeting.RoomID = request.NewRoomID.Value;

            _classMeetingRepo.Update(meeting);

            // Now handle syllabus item shifting
            // If the meeting is moved to a later date, shift syllabus items forward
            if (request.ToMeetingDate.Value > originalDate)
            {
                // Get the meetings between original date and new date (excluding the rescheduled one)
                var meetingsBetween = futureMeetings
                    .Where(m => m.Id != meeting.Id && m.Date > originalDate && m.Date <= request.ToMeetingDate.Value)
                    .OrderBy(m => m.Date)
                    .ToList();

                if (meetingsBetween.Any())
                {
                    // Shift syllabus items forward for meetings between original and new date
                    // Each meeting takes the syllabus item from the previous meeting
                    var previousTopicId = originalCoveredTopicId;
                    
                    foreach (var meetingBetween in meetingsBetween)
                    {
                        var tempTopicId = meetingBetween.CoveredTopicID;
                        meetingBetween.CoveredTopicID = previousTopicId;
                        _classMeetingRepo.Update(meetingBetween);
                        previousTopicId = tempTopicId;
                    }

                    // The rescheduled meeting gets the last syllabus item from the chain
                    meeting.CoveredTopicID = previousTopicId;
                    _classMeetingRepo.Update(meeting);
                }
            }
            // If the meeting is moved to an earlier date, shift syllabus items backward
            else if (request.ToMeetingDate.Value < originalDate)
            {
                // Get meetings between new date and original date (excluding the rescheduled one)
                var meetingsBetween = futureMeetings
                    .Where(m => m.Id != meeting.Id && m.Date >= request.ToMeetingDate.Value && m.Date < originalDate)
                    .OrderByDescending(m => m.Date)
                    .ToList();

                if (meetingsBetween.Any())
                {
                    // Shift syllabus items backward for meetings between new and original date
                    // Each meeting takes the syllabus item from the next meeting
                    var nextTopicId = originalCoveredTopicId;
                    
                    foreach (var meetingBetween in meetingsBetween)
                    {
                        var tempTopicId = meetingBetween.CoveredTopicID;
                        meetingBetween.CoveredTopicID = nextTopicId;
                        _classMeetingRepo.Update(meetingBetween);
                        nextTopicId = tempTopicId;
                    }

                    // The rescheduled meeting gets the first syllabus item from the chain
                    meeting.CoveredTopicID = nextTopicId;
                    _classMeetingRepo.Update(meeting);
                }
            }
            // If same date but different slot, no syllabus shift needed
        }

        private async Task HandleSuspensionApprovalAsync(ACAD_AcademicRequest request)
        {
         
            if (!request.SuspensionStartDate.HasValue || !request.SuspensionEndDate.HasValue)
            {
                throw new InvalidOperationException("Suspension dates are required for suspension approval.");
            }

            // Set ExpectedReturnDate if not already set
            if (!request.ExpectedReturnDate.HasValue)
            {
                request.ExpectedReturnDate = request.SuspensionEndDate.Value.AddDays(1);
            }
        }

        private async Task HandleDropoutCompletionAsync(ACAD_AcademicRequest request)
        {
            var account = await _accountRepo.GetDetailByIdAsync(request.StudentID);
            if (account == null)
            {
                throw new KeyNotFoundException("Student account not found.");
            }

            var droppedOutStatus = await _lookUpRepository.GetByCodeAsync(LookUpTypes.AccountStatus, "Dropped");
            if (droppedOutStatus == null)
            {
                throw new KeyNotFoundException("Dropped status not found in lookup data.");
            }

            account.AccountStatusID = droppedOutStatus.Id;
            _accountRepo.Update(account);

            if (request.EnrollmentID.HasValue)
            {
                var enrollment = await _enrollmentRepo.GetByIdAsync(request.EnrollmentID.Value);
                if (enrollment != null)
                {
                    var pendingReservationStatus = await _lookUpRepository.GetByCodeAsync(LookUpTypes.ReservationStatus, "Pending");
                    var cancelledReservationStatus = await _lookUpRepository.GetByCodeAsync(LookUpTypes.ReservationStatus, "Cancelled");

                    if (pendingReservationStatus == null)
                    {
                        throw new KeyNotFoundException("Pending reservation status not found in lookup data.");
                    }

                    if (cancelledReservationStatus == null)
                    {
                        throw new KeyNotFoundException("Cancelled reservation status not found in lookup data.");
                    }

                    var reservations = _classReservationRepo.GetReservationByStudentId(request.StudentID).ToList();

                    foreach (var reservation in reservations)
                    {
                        if (reservation.ReservationStatusID != pendingReservationStatus.Id)
                        {
                            continue; 
                        }

                        var hasCourse = reservation.ACAD_ReservationItems
                            .Any(item => item.CourseID == enrollment.CourseID);

                        if (!hasCourse)
                        {
                            continue;
                        }

                        reservation.ReservationStatusID = cancelledReservationStatus.Id;
                        _classReservationRepo.Update(reservation);
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }

        private async Task HandleEnrollmentCancellationAsync(ACAD_AcademicRequest request)
        {
            // When an enrollment cancellation is approved, cancel the pending enrollment
            if (!request.EnrollmentID.HasValue)
            {
                throw new InvalidOperationException("EnrollmentID is required for enrollment cancellation.");
            }

            var enrollment = await _enrollmentRepo.GetByIdAsync(request.EnrollmentID.Value);
            if (enrollment == null)
            {
                throw new KeyNotFoundException("Enrollment not found.");
            }

            // Get the Cancelled enrollment status
            var cancelledStatus = await _lookUpRepository.GetByCodeAsync(LookUpTypes.EnrollmentStatus, "Cancelled");
            if (cancelledStatus == null)
            {
                // If "Cancelled" doesn't exist, try "Dropped" as fallback
                cancelledStatus = await _lookUpRepository.GetByCodeAsync(LookUpTypes.EnrollmentStatus, "Dropped");
                if (cancelledStatus == null)
                {
                    throw new KeyNotFoundException("Cancelled or Dropped enrollment status not found in lookup data.");
                }
            }
            // Update enrollment status to Cancelled and remove class assignment
            enrollment.EnrollmentStatusID = cancelledStatus.Id;
            enrollment.ClassID = null; // Remove class assignment when cancelled
           
            _enrollmentRepo.Update(enrollment);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task HandleReturnFromSuspensionAsync(ACAD_AcademicRequest request)
        {
            // When a return from suspension is approved, reactivate the enrollment
            if (!request.EnrollmentID.HasValue)
            {
                throw new InvalidOperationException("EnrollmentID is required for return from suspension.");
            }

            var enrollment = await _enrollmentRepo.GetByIdAsync(request.EnrollmentID.Value);
            if (enrollment == null)
            {
                throw new KeyNotFoundException("Enrollment not found.");
            }

            // Verify enrollment is suspended or awaiting return
            var currentStatus = await _lookUpRepository.GetByIdAsync(enrollment.EnrollmentStatusID);
            var statusCode = (currentStatus?.Code ?? "").ToLower();
            
            if (statusCode != "suspended" && statusCode != "awaitingreturn")
            {
                throw new InvalidOperationException(
                    $"Only suspended or awaiting return enrollments can be reactivated. Current status: {currentStatus?.Name ?? "Unknown"}"
                );
            }

            // Get Enrolled status
            var enrolledStatus = await _lookUpRepository.GetByCodeAsync(LookUpTypes.EnrollmentStatus, "Enrolled");
            if (enrolledStatus == null)
            {
                throw new KeyNotFoundException("Enrolled enrollment status not found in lookup data.");
            }

            // Reactivate enrollment
            enrollment.EnrollmentStatusID = enrolledStatus.Id;
            
            _enrollmentRepo.Update(enrollment);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task HandleRefundApprovalAsync(ACAD_AcademicRequest request)
        {
            // When a refund request is approved, mark the related enrollment as Refunded
            if (!request.EnrollmentID.HasValue)
            {
                throw new InvalidOperationException("EnrollmentID is required for refund requests.");
            }

            var enrollment = await _enrollmentRepo.GetByIdAsync(request.EnrollmentID.Value);
            if (enrollment == null)
            {
                throw new KeyNotFoundException("Enrollment not found.");
            }

            // Get the Refunded enrollment status
            var refundedStatus = await _lookUpRepository.GetByCodeAsync(LookUpTypes.EnrollmentStatus, "Refunded");
            if (refundedStatus == null)
            {
                throw new KeyNotFoundException("Refunded enrollment status not found in lookup data.");
            }

            // Update enrollment status to Refunded
            enrollment.EnrollmentStatusID = refundedStatus.Id;

            _enrollmentRepo.Update(enrollment);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task HandleClassTransferAsync(ACAD_AcademicRequest request)
        {
            if (!request.EnrollmentID.HasValue || !request.ToClassID.HasValue)
                throw new InvalidOperationException("Invalid class transfer request.");

            var enrollment = await _enrollmentRepo.GetByIdAsync(request.EnrollmentID.Value);
            if (enrollment == null)
                throw new KeyNotFoundException("Enrollment not found.");

            enrollment.ClassID = request.ToClassID.Value;

            _enrollmentRepo.Update(enrollment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<AcademicRequestResponse>> GetRequestsByStudentAsync(Guid studentId)
        {
            var requests = await _requestRepo.GetByStudentAsync(studentId);
            return _mapper.Map<IEnumerable<AcademicRequestResponse>>(requests);
        }

        public async Task<IEnumerable<AcademicRequestHistoryResponse>> GetRequestHistoryAsync(Guid requestId)
        {
            var history = await _historyRepo.GetByRequestAsync(requestId);
            return _mapper.Map<IEnumerable<AcademicRequestHistoryResponse>>(history);
        }
        public async Task<AcademicRequestResponse?> GetDetailsAsync(Guid requestId)
        {
            var r = await _requestRepo.GetDetailsAsync(requestId);
            return _mapper.Map<AcademicRequestResponse?>(r);
        }

        public async Task<IEnumerable<AcademicRequestResponse>> GetAllRequestsAsync()
        {
            var requests = await _requestRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<AcademicRequestResponse>>(requests);
        }

        public async Task<IEnumerable<AcademicRequestResponse>> GetRequestsByStatusAsync(Guid statusId)
        {
            var requests = await _requestRepo.GetByStatusAsync(statusId);
            return _mapper.Map<IEnumerable<AcademicRequestResponse>>(requests);
        }

        public async Task<AcademicRequestUploadResponse> GetAttachmentUploadUrlAsync(string fileName, string contentType)
        {
            // Get presigned upload URL and generated file path
            var (uploadUrl, filePath) = await _fileStorageService.GetPresignedPutUrlAsync("academic-requests", fileName, contentType);

            return new AcademicRequestUploadResponse
            {
                UploadUrl = uploadUrl,
                FilePath = filePath
            };
        }

        public async Task<string> GetAttachmentDownloadUrlAsync(string filePath)
        {
            // Get presigned download URL for the attachment
            return await _fileStorageService.GetPresignedGetUrlAsync(filePath);
        }

        public async Task UpdateAttachmentAsync(UpdateAcademicRequestAttachment requestDto)
        {
            var entity = await _requestRepo.GetByIdAsync(requestDto.RequestID);
            if (entity == null)
                throw new KeyNotFoundException("Request not found");

            // Check if request status allows updates (only NeedInfo status)
            var status = await _lookUpRepository.GetByIdAsync(entity.AcademicRequestStatusID);
            var statusName = (status?.Name ?? "").ToLower();
            
            if (statusName != "needinfo" && statusName != "need info")
            {
                throw new InvalidOperationException("Attachment can only be updated when request status is 'Need Info'");
            }

            // Get Pending status to change back to
            var pendingStatus = await _lookUpRepository.GetByCodeAsync(LookUpTypes.AcademicRequestStatus, "Pending");
            if (pendingStatus == null)
            {
                throw new KeyNotFoundException("Pending status not found. Please ensure the lookup data is properly seeded.");
            }

            // Update attachment URL
            entity.AttachmentUrl = requestDto.AttachmentUrl;
            
            // Change status back to Pending so staff knows to review
            entity.AcademicRequestStatusID = pendingStatus.Id;
            
            // Create history entry
            var historyDescription = !string.IsNullOrEmpty(requestDto.AdditionalNotes)
                ? $"Student updated attachment: {requestDto.AdditionalNotes}"
                : "Student updated attachment";
            
            var history = new ACAD_AcademicRequestHistory
            {
                RequestID = entity.Id,
                StatusID = pendingStatus.Id,
                AttachmentUrl = requestDto.AttachmentUrl
            };
            _historyRepo.Add(history);

            _requestRepo.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task SendRequestStatusNotificationAsync(ACAD_AcademicRequest request, CORE_LookUp status)
        {
            try
            {
                var requestType = await _lookUpRepository.GetByIdAsync(request.RequestTypeID);
                var requestTypeName = requestType?.Name ?? "Academic Request";
                
                var statusName = status.Name?.ToLower();
                var isApproved = statusName == "approved";
                var isRejected = statusName == "rejected";

                if (!isApproved && !isRejected)
                    return; // Only send notifications for approved/rejected status

                var title = isApproved 
                    ? $"✅ {requestTypeName} Request Approved"
                    : $"❌ {requestTypeName} Request Rejected";

                var message = isApproved
                    ? $"Great news! Your {requestTypeName.ToLower()} request has been approved by staff. "
                    : $"Your {requestTypeName.ToLower()} request has been rejected by staff. ";

                // Add staff response if available
                if (!string.IsNullOrEmpty(request.StaffResponse))
                {
                    message += $"Staff comment: {request.StaffResponse}";
                }
                else
                {
                    message += isApproved 
                        ? "The changes will be processed accordingly."
                        : "Please review the requirements and submit a new request if needed.";
                }

                var notificationRequest = new CreateNotificationRequest
                {
                    UserId = request.StudentID.ToString().ToUpperInvariant(),
                    Title = title,
                    Message = message,
                    Type = isApproved ? "info" : "warning",
                    IsRead = false
                };

                await _notificationService.CreateAsync(notificationRequest);
            }
            catch (Exception ex)
            {
                // Log the error but don't fail the request processing
                // In a real application, you would use a proper logging framework
                Console.WriteLine($"Failed to send notification for request {request.Id}: {ex.Message}");
            }
        }

        private async Task SendDropoutRequestSubmittedEmailAsync(ACAD_AcademicRequest request, CORE_LookUp requestType)
        {
            try
            {
                var student = await _accountRepo.GetByIdAsync(request.StudentID);
                if (student == null || string.IsNullOrEmpty(student.Email))
                    return;

                var subject = "Dropout Request Submitted - CETS";
                var effectiveDate = request.EffectiveDate?.ToString("dd MMM yyyy") ?? "Not specified";
                var submissionDate = DateTime.Now.ToString("dd MMM yyyy");

                var body = _emailTemplateBuilder.BuildDropoutRequestSubmittedEmail(
                    studentName: student.FullName ?? "Student",
                    requestType: requestType?.Name ?? "Dropout Request",
                    effectiveDate: effectiveDate,
                    submissionDate: submissionDate
                );

                await _mailService.SendEmailAsync(student.Email, subject, body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send dropout submission email for request {request.Id}: {ex.Message}");
            }
        }

        private async Task SendDropoutRequestStatusEmailAsync(ACAD_AcademicRequest request, CORE_LookUp status)
        {
            try
            {
                var student = await _accountRepo.GetByIdAsync(request.StudentID);
                if (student == null || string.IsNullOrEmpty(student.Email))
                    return;

                var requestType = await _lookUpRepository.GetByIdAsync(request.RequestTypeID);
                var statusName = status.Name?.ToLower();
                var isApproved = statusName == "approved";
                var isCompleted = statusName == "completed";
                
                var subject = isApproved 
                    ? "Dropout Request Approved - CETS"
                    : isCompleted 
                        ? "Dropout Request Completed - CETS"
                        : $"Dropout Request {status.Name} - CETS";

                var effectiveDate = request.EffectiveDate?.ToString("MMMM dd, yyyy") ?? "Not specified";
                var processedDate = DateTime.Now.ToString("MMMM dd, yyyy");

                string body;
                if (isApproved)
                {
                    body = _emailTemplateBuilder.BuildDropoutRequestApprovedEmail(
                        studentName: student.FullName ?? "Student",
                        requestType: requestType?.Name ?? "Dropout Request",
                        effectiveDate: effectiveDate,
                        status: status.Name ?? "Approved",
                        processedDate: processedDate,
                        staffComment: request.StaffResponse
                    );
                }
                else if (isCompleted)
                {
                    body = _emailTemplateBuilder.BuildDropoutRequestCompletedEmail(
                        studentName: student.FullName ?? "Student",
                        requestType: requestType?.Name ?? "Dropout Request",
                        effectiveDate: effectiveDate,
                        status: status.Name ?? "Completed",
                        processedDate: processedDate,
                        staffComment: request.StaffResponse
                    );
                }
                else
                {
                    // For other statuses, skip email
                    return;
                }

                await _mailService.SendEmailAsync(student.Email, subject, body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send dropout status email for request {request.Id}: {ex.Message}");
            }
        }
    }
}
