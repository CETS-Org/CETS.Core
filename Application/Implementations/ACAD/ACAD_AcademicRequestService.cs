using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ACAD_AcademicRequestService(
            IACAD_AcademicRequestRepository requestRepo,
            IACAD_AcademicRequestHistoryRepository historyRepo,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _requestRepo = requestRepo;
            _historyRepo = historyRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AcademicRequestResponse> SubmitRequestAsync(CreateAcademicRequest requestDto)
        {
            var entity = _mapper.Map<ACAD_AcademicRequest>(requestDto);
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.AcademicRequestStatusID = Guid.Empty; 

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
            entity.ProcessedAt = DateTime.UtcNow;
            _requestRepo.Update(entity);

            var history = _mapper.Map<ACAD_AcademicRequestHistory>(requestDto);
            history.Id = Guid.NewGuid();
            history.RequestID = requestDto.RequestID;
            history.UpdatedAt = DateTime.UtcNow;

            _historyRepo.Add(history);
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
    }
}
