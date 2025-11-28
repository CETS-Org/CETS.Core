using DTOs.ACAD.ACAD_AcademicRequest.Requests;
using DTOs.ACAD.ACAD_AcademicRequest.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_SuspensionValidationService
    {
        Task<SuspensionValidationResult> ValidateSuspensionRequestAsync(CreateSuspensionRequest request);
        Task<SuspensionValidationResult> ValidateSuspensionRequestAsync(CreateAcademicRequest request);
    }
}

