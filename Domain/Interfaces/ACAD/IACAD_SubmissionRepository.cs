using Domain.Entities;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_SubmissionRepository : IBaseRepository<ACAD_Submission>
    {
        Task<IEnumerable<ACAD_Submission>> GetByAssignmentAsync(Guid assignmentId);
        Task<IEnumerable<ACAD_Submission>> GetByStudentAsync(Guid studentId);
    }
}


