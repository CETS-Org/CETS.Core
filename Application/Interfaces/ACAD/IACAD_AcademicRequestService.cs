using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_AcademicRequestService
    {
        Task<ACAD_AcademicRequest> SubmitRequestAsync(
            Guid studentId, Guid requestTypeId, string reason, Guid fromClassId, Guid? toClassId = null);
        Task ProcessRequestAsync(
            Guid requestId, Guid statusId, string description, Guid staffId, string? attachmentUrl = null);
        Task<IEnumerable<ACAD_AcademicRequest>> GetRequestsByStudentAsync(Guid studentId);
        Task<IEnumerable<ACAD_AcademicRequestHistory>> GetRequestHistoryAsync(Guid requestId);
    }
}
