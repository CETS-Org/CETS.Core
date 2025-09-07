using Domain.Entities;
using DTOs.ACAD.ACAD_Submission.Requests;
using DTOs.ACAD.ACAD_Submission.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface ISubmissionService
    {
        Task<SubmissionResponse> SubmitAssignmentAsync(SubmitAssignmentRequest request);
        Task<IEnumerable<SubmissionResponse>> GetSubmissionsByAssignmentAsync(Guid assignmentId);
        Task<IEnumerable<SubmissionResponse>> GetSubmissionsByStudentAsync(Guid studentId);
        Task<SubmissionResponse> GradeSubmissionAsync(GradeSubmissionRequest request);
    }

}
