using Domain.Entities;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_AssignmentRepository : IBaseRepository<ACAD_Assignment>
    {
        Task<IEnumerable<ACAD_Assignment>> GetByClassMeetingAsync(Guid classMeetingId);
        Task<IEnumerable<ACAD_Assignment>> GetByTeacherAsync(Guid teacherId);
        Task<IEnumerable<ACAD_Assignment>> GetAssignmentsWithSubmissions(Guid classMeetingId, Guid studentId);
    }

}


