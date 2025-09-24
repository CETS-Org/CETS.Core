using Domain.Entities;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_CourseTeacherAssignmentRepository : IBaseRepository<ACAD_CourseTeacherAssignment>
    {
        Task<IEnumerable<ACAD_Course>> GetCoursesByTeacherIdAsync(Guid teacherId);
        Task<IEnumerable<ACAD_CourseTeacherAssignment>> GetCourseTeacherAssignmentsByTeacherIdAsync(Guid teacherId);
        Task<IEnumerable<ACAD_CourseTeacherAssignment>> GetCourseTeacherAssignmentsByTeacherIdAndCourseIdAsync(Guid teacherId, Guid courseId);
    }
}


