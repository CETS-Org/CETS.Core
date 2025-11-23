using Domain.Entities;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_EnrollmentRepository : IBaseRepository<ACAD_Enrollment>
    {
        Task<IEnumerable<ACAD_Enrollment>> GetByStudentAsync(Guid studentId);
        Task<IEnumerable<ACAD_Enrollment>> GetByClassAsync(Guid classId);
        Task<ACAD_Enrollment?> GetDetailAsync(Guid enrollmentId);
        Task<IEnumerable<ACAD_Enrollment>> GetStudentAcademicResultsAsync(Guid studentId);
        Task<ACAD_Enrollment?> GetEnrollmentDetailByStudentAndCourseAsync(Guid studentId, Guid courseId);

        Task<IEnumerable<ACAD_Enrollment>> GetStudentWaitList(Guid courseId);

    }
}


