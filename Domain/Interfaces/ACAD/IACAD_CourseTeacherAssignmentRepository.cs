using Domain.Entities;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_CourseTeacherAssignmentRepository : IBaseRepository<ACAD_CourseTeacherAssignment>
    {
        Task<IEnumerable<ACAD_Course>> GetCoursesByTeacherIdAsync(Guid teacherId);
    }
}


