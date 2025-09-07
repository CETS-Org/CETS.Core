using Domain.Entities;
using DTOs.ACAD.ACAD_AcademicRequest.Requests;
using DTOs.ACAD.ACAD_AcademicRequest.Responses;
using DTOs.ACAD.ACAD_AcademicRequestHistory.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_AcademicRequestService
    {
        Task<AcademicRequestResponse> SubmitRequestAsync(CreateAcademicRequest requestDto);

        Task ProcessRequestAsync(ProcessAcademicRequest requestDto);

        Task<IEnumerable<AcademicRequestResponse>> GetRequestsByStudentAsync(Guid studentId);

        Task<IEnumerable<AcademicRequestHistoryResponse>> GetRequestHistoryAsync(Guid requestId);
    }
}
