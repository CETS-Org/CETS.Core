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
            // TODO: Add proper role-based validation for meeting reschedule requests
            // Currently, the frontend filters out meeting reschedule for students
            // For proper backend validation, we need to check the user's role from the authentication context
            // or pass the user's role in the request DTO

            // Get request type to check if it's a suspension request
            var requestType = await _lookUpRepository.GetByIdAsync(requestDto.RequestTypeID);
            if (requestType == null)
            {
                throw new KeyNotFoundException("Request type not found. Please ensure the lookup data is properly seeded.");
            }

            var requestTypeName = (requestType.Name ?? "").ToLower();
            var requestTypeCode = (requestType.Code ?? "").ToLower();
            var isSuspension = requestTypeName.Contains("suspension") || requestTypeCode.Contains("suspension");
            var isDropout = requestTypeName.Contains("dropout") || requestTypeCode.Contains("dropout") || 
                           requestTypeName.Contains("dropping out") || requestTypeCode.Contains("droppingout");

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

                if (requestTypeName.Contains("meeting reschedule") || requestTypeCode.Contains("meetingreschedule") ||
                    requestTypeName.Contains("class transfer") || requestTypeCode.Contains("classtransfer"))
                {
                    priorityCode = "High";
                }
                else if (requestTypeName.Contains("enrollment cancellation") || requestTypeCode.Contains("enrollmentcancellation") ||
                         requestTypeName.Contains("suspension") || requestTypeCode.Contains("suspension") ||
                         requestTypeName.Contains("dropout") || requestTypeCode.Contains("dropout"))
                {
                    priorityCode = "Medium";
                }
                else if (requestTypeName.Contains("other") || requestTypeCode.Contains("other"))
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
            if (requestTypeName.Contains("meeting reschedule") || requestTypeCode.Contains("meetingreschedule"))
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
            var requestTypeName = (requestType?.Name ?? "").ToLower();
            var requestTypeCode = (requestType?.Code ?? "").ToLower();
            var isSuspension = requestTypeName.Contains("suspension") || requestTypeCode.Contains("suspension");
            var isDropout = requestTypeName.Contains("dropout") || requestTypeCode.Contains("dropout") ;
            
            if (approvedStatus != null && requestDto.StatusID == approvedStatus.Id && isSuspension)
            {
                await HandleSuspensionApprovalAsync(entity);
            }

            // If this is a completed dropout request, handle dropout finalization
            var completedStatus = await _lookUpRepository.GetByCodeAsync(LookUpTypes.AcademicRequestStatus, "Completed");
            if (completedStatus != null && requestDto.StatusID == completedStatus.Id && isDropout)
            {
                await HandleDropoutCompletionAsync(entity);
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
            // When a suspension request is approved, we need to:
            // 1. Update the student's account status to "Suspended" on the start date (handled by background job/scheduler)
            // 2. Set the expected return date
            // 3. Schedule reminders (handled by notification service/background job)
            
            // Note: The actual status change happens on the SuspensionStartDate
            // This method just validates and prepares the suspension

            if (!request.SuspensionStartDate.HasValue || !request.SuspensionEndDate.HasValue)
            {
                throw new InvalidOperationException("Suspension dates are required for suspension approval.");
            }

            // Set ExpectedReturnDate if not already set
            if (!request.ExpectedReturnDate.HasValue)
            {
                request.ExpectedReturnDate = request.SuspensionEndDate.Value.AddDays(1);
            }

            // Note: A background job should:
            // - On SuspensionStartDate: Set account status to "Suspended" and request status to "Suspended"
            // - 3 days before ExpectedReturnDate: Send reminder notification
            // - On ExpectedReturnDate: Send return notification and set status to "AwaitingReturn"
            // - After AwaitingReturnGraceDays: Optionally set to "AutoDroppedOut"
        }

        private async Task HandleDropoutCompletionAsync(ACAD_AcademicRequest request)
        {
            // When a dropout request is completed (final step), we need to:
            // 1. Update the student's account status to "DroppedOut"
            // 2. Clear class assignments (if any)
            // 3. Stop attendance tracking
            // 4. Apply refund policy if applicable (handled separately by finance module)
            
            // Note: This is a permanent action and cannot be undone
            // Student must re-enroll as a new student if they want to return

            // Get the student account
            var account = await _accountRepo.GetDetailByIdAsync(request.StudentID);
            if (account == null)
            {
                throw new KeyNotFoundException("Student account not found.");
            }

            // Get the DroppedOut status
            var droppedOutStatus = await _lookUpRepository.GetByCodeAsync(LookUpTypes.AccountStatus, "Dropped");
            if (droppedOutStatus == null)
            {
                throw new KeyNotFoundException("Dropped status not found in lookup data.");
            }

            // Update student account status to DroppedOut
            account.AccountStatusID = droppedOutStatus.Id;
            _accountRepo.Update(account);

            // Note: Additional actions should be handled by background jobs or separate services:
            // - Remove student from class roster
            // - Cancel upcoming sessions/enrollments
            // - Freeze tuition calculations
            // - Process refunds if applicable based on refund policy
            // - Deactivate LMS access (optional)
            // - Send final confirmation email

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<AcademicRequestResponse>> GetRequestsByStudentAsync(Guid studentId)
        {
            var requests = await _requestRepo.GetByStudentAsync(studentId);
            await UpdateExpiredRequestsAsync(requests);
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
            if (r != null)
            {
                await UpdateExpiredRequestsAsync(new[] { r });
            }
            return _mapper.Map<AcademicRequestResponse?>(r);
        }

        public async Task<IEnumerable<AcademicRequestResponse>> GetAllRequestsAsync()
        {
            var requests = await _requestRepo.GetAllAsync();
            await UpdateExpiredRequestsAsync(requests);
            return _mapper.Map<IEnumerable<AcademicRequestResponse>>(requests);
        }

        public async Task<IEnumerable<AcademicRequestResponse>> GetRequestsByStatusAsync(Guid statusId)
        {
            var requests = await _requestRepo.GetByStatusAsync(statusId);
            await UpdateExpiredRequestsAsync(requests);
            return _mapper.Map<IEnumerable<AcademicRequestResponse>>(requests);
        }

        // Helper method to update expired academic requests
        private async Task UpdateExpiredRequestsAsync(IEnumerable<ACAD_AcademicRequest> requests)
        {
            var expiredStatus = await _lookUpRepository.GetByCodeAsync(LookUpTypes.AcademicRequestStatus, "Expired");
            if (expiredStatus == null)
            {
                // If Expired status doesn't exist, skip the update
                return;
            }

            var pendingStatus = await _lookUpRepository.GetByCodeAsync(LookUpTypes.AcademicRequestStatus, "Pending");
            if (pendingStatus == null)
            {
                return;
            }

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var hasChanges = false;

            foreach (var request in requests)
            {
                // Only update pending requests that have passed their effective date
                if (request.AcademicRequestStatusID == pendingStatus.Id && 
                    request.EffectiveDate.HasValue && 
                    request.EffectiveDate.Value < today)
                {
                    request.AcademicRequestStatusID = expiredStatus.Id;
                    _requestRepo.Update(request);
                    hasChanges = true;
                }
            }

            if (hasChanges)
            {
                await _unitOfWork.SaveChangesAsync();
            }
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
                var effectiveDate = request.EffectiveDate?.ToString("MMMM dd, yyyy") ?? "Not specified";
                var submissionDate = DateTime.Now.ToString("MMMM dd, yyyy");

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
