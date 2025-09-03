using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface ISubmissionService
    {
        Task<ACAD_Submission> SubmitAssignmentAsync(ACAD_Submission submission);
        Task<IEnumerable<ACAD_Submission>> GetSubmissionsByAssignmentAsync(Guid assignmentId);
        Task<IEnumerable<ACAD_Submission>> GetSubmissionsByStudentAsync(Guid studentId);
        Task GradeSubmissionAsync(Guid submissionId, decimal score, string? feedback);
    }
}
