using Application.Interfaces.ACAD;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ACAD_AcademicRequestService(
            IACAD_AcademicRequestRepository requestRepo,
            IACAD_AcademicRequestHistoryRepository historyRepo,
            ICORE_LookUpRepository lookUpRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _requestRepo = requestRepo;
            _historyRepo = historyRepo;
            _lookUpRepository = lookUpRepository;
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

            return _mapper.Map<AcademicRequestResponse>(entity);
        }

        public async Task ProcessRequestAsync(ProcessAcademicRequest requestDto)
        {
            var entity = await _requestRepo.GetByIdAsync(requestDto.RequestID);
            if (entity == null)
                throw new KeyNotFoundException("Request not found");

            entity.AcademicRequestStatusID = requestDto.StatusID;
            entity.ProcessedBy = requestDto.StaffID;
            _requestRepo.Update(entity);

            var history = _mapper.Map<ACAD_AcademicRequestHistory>(requestDto);
            history.RequestID = requestDto.RequestID;

            _historyRepo.Add(history);
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
    }
}
