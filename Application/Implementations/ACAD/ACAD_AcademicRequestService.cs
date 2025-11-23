using Application.Interfaces.ACAD;
using Application.Interfaces.Common.Storage;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using Domain.Interfaces.CORE;
using DTOs.ACAD.ACAD_AcademicRequest.Requests;
using DTOs.ACAD.ACAD_AcademicRequest.Responses;
using DTOs.ACAD.ACAD_AcademicRequestHistory.Responses;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ACAD_AcademicRequestService(
            IACAD_AcademicRequestRepository requestRepo,
            IACAD_AcademicRequestHistoryRepository historyRepo,
            ICORE_LookUpRepository lookUpRepository,
            IFileStorageService fileStorageService,
            IACAD_ClassMeetingRepository classMeetingRepo,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _requestRepo = requestRepo;
            _historyRepo = historyRepo;
            _lookUpRepository = lookUpRepository;
            _fileStorageService = fileStorageService;
            _classMeetingRepo = classMeetingRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AcademicRequestResponse> SubmitRequestAsync(CreateAcademicRequest requestDto)
        {
            var entity = _mapper.Map<ACAD_AcademicRequest>(requestDto);
            
            var pendingStatus = await _lookUpRepository.GetByCodeAsync(LookUpTypes.AcademicRequestStatus, "Pending");
            if (pendingStatus == null)
            {
                throw new KeyNotFoundException("Pending status not found for AcademicRequestStatus. Please ensure the lookup data is properly seeded.");
            }
            
            entity.AcademicRequestStatusID = pendingStatus.Id;

            if (entity.FromClassID.HasValue || entity.ToClassID.HasValue)
            {
                entity.EffectiveDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7));
            }

            _requestRepo.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            // Create history entry for request submission
            var history = new ACAD_AcademicRequestHistory
            {
                RequestID = entity.Id,
                StatusID = pendingStatus.Id,
                Description = "Request submitted",
                AttachmentUrl = requestDto.AttachmentUrl
            };

            _historyRepo.Add(history);
            await _unitOfWork.SaveChangesAsync();

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
                await HandleMeetingRescheduleAsync(entity);
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
        }

        private async Task HandleMeetingRescheduleAsync(ACAD_AcademicRequest request)
        {
            if (!request.ClassMeetingID.HasValue || !request.NewMeetingDate.HasValue)
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
            meeting.Date = request.NewMeetingDate.Value;
            if (request.NewSlotID.HasValue)
                meeting.SlotID = request.NewSlotID.Value;
            if (request.NewRoomID.HasValue)
                meeting.RoomID = request.NewRoomID.Value;

            _classMeetingRepo.Update(meeting);

            // Now handle syllabus item shifting
            // If the meeting is moved to a later date, shift syllabus items forward
            if (request.NewMeetingDate.Value > originalDate)
            {
                // Get the meetings between original date and new date (excluding the rescheduled one)
                var meetingsBetween = futureMeetings
                    .Where(m => m.Id != meeting.Id && m.Date > originalDate && m.Date <= request.NewMeetingDate.Value)
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
            else if (request.NewMeetingDate.Value < originalDate)
            {
                // Get meetings between new date and original date (excluding the rescheduled one)
                var meetingsBetween = futureMeetings
                    .Where(m => m.Id != meeting.Id && m.Date >= request.NewMeetingDate.Value && m.Date < originalDate)
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
    }
}
