using Domain.Entities;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_EnrollmentRepository : IBaseRepository<ACAD_Enrollment>
    {
        Task<IEnumerable<ACAD_Enrollment>> GetByStudentAsync(Guid studentId);
        Task<IEnumerable<ACAD_Enrollment>> GetByClassAsync(Guid classId);
        Task<ACAD_Enrollment?> GetDetailAsync(Guid enrollmentId);
    }
}


