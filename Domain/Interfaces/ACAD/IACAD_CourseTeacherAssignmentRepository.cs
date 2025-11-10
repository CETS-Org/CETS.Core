using Domain.Entities;
using DTOs.ACAD.ACAD_Course.Responses;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_CourseTeacherAssignmentRepository : IBaseRepository<ACAD_CourseTeacherAssignment>
    {
        Task<IEnumerable<TeachingCourseResponse>> GetCoursesByTeacherIdAsync(Guid teacherId);
        Task<IEnumerable<ACAD_CourseTeacherAssignment>> GetCourseTeacherAssignmentsByTeacherIdAsync(Guid teacherId);
        Task<IEnumerable<ACAD_CourseTeacherAssignment>> GetCourseTeacherAssignmentsByTeacherIdAndCourseIdAsync(Guid teacherId, Guid courseId);
    }
}


