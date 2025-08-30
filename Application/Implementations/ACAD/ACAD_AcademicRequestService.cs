using Application.Interfaces.ACAD;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.ACAD
{
    public class ACAD_AcademicRequestService : IACAD_AcademicRequestService
    {
        private readonly IACAD_AcademicRequestRepository _requestRepo;
        private readonly IACAD_AcademicRequestHistoryRepository _historyRepo;
        private readonly IUnitOfWork _unitOfWork;

        public ACAD_AcademicRequestService(
            IACAD_AcademicRequestRepository requestRepo,
            IACAD_AcademicRequestHistoryRepository historyRepo,
            IUnitOfWork unitOfWork)
        {
            _requestRepo = requestRepo;
            _historyRepo = historyRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<ACAD_AcademicRequest> SubmitRequestAsync(
            Guid studentId, Guid requestTypeId, string reason, Guid fromClassId, Guid? toClassId = null)
        {
            var request = new ACAD_AcademicRequest
            {
                Id = Guid.NewGuid(),
                StudentID = studentId,
                RequestTypeID = requestTypeId,
                AcademicRequestStatusID = Guid.Empty, 
                Reason = reason,
                FromClassID = fromClassId,
                ToClassID = toClassId,
                CreatedAt = DateTime.UtcNow
            };

            _requestRepo.Add(request);
            await _unitOfWork.SaveChangesAsync();
            return request;
        }

        public async Task ProcessRequestAsync(
            Guid requestId, Guid statusId, string description, Guid staffId, string? attachmentUrl = null)
        {
            var request = await _requestRepo.GetByIdAsync(requestId);
            if (request == null) throw new Exception("Request not found");

            request.AcademicRequestStatusID = statusId;
            request.ProcessedBy = staffId;
            request.ProcessedAt = DateTime.UtcNow;
            _requestRepo.Update(request);

            var history = new ACAD_AcademicRequestHistory
            {
                Id = Guid.NewGuid(),
                RequestID = requestId,
                StatusID = statusId,
                Description = description,
                UpdatedBy = staffId,
                UpdatedAt = DateTime.UtcNow,
                AttachmentUrl = attachmentUrl
            };

            _historyRepo.Add(history);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<ACAD_AcademicRequest>> GetRequestsByStudentAsync(Guid studentId)
        {
            return await _requestRepo.GetByStudentAsync(studentId);
        }

        public async Task<IEnumerable<ACAD_AcademicRequestHistory>> GetRequestHistoryAsync(Guid requestId)
        {
            return await _historyRepo.GetByRequestAsync(requestId);
        }
    }
}
