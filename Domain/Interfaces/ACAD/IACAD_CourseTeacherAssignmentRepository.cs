using Domain.Entities;
using DTOs.ACAD.ACAD_Course.Responses;
using DTOs.ACAD.ACAD_CourseTeacherAssignment.Requests;
using DTOs.ACAD.ACAD_CourseTeacherAssignment.Responses;
using DTOs.IDN.IDN_Teacher.Responses;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_CourseTeacherAssignmentRepository : IBaseRepository<ACAD_CourseTeacherAssignment>
    {
        Task<IEnumerable<TeachingCourseResponse>> GetCoursesByTeacherIdAsync(Guid teacherId);
        Task<IEnumerable<ACAD_CourseTeacherAssignment>> GetCourseTeacherAssignmentsByTeacherIdAsync(Guid teacherId);
        Task<IEnumerable<ACAD_CourseTeacherAssignment>> GetCourseTeacherAssignmentsByTeacherIdAndCourseIdAsync(Guid teacherId, Guid courseId);
        Task<IEnumerable<IDN_Teacher>> GetTeachersByCourseAsync(Guid courseId);
        Task<IEnumerable<ACAD_CourseTeacherAssignment>> GetTeacherAssignmentByCourseAsync(Guid courseId);
        Task<IEnumerable<ACAD_CourseTeacherAssignment>> GetCourseTeacherAssignmentsByCourseIdAsync(Guid courseId);
        Task<ACAD_CourseTeacherAssignment?> GetAssignmentWithDetailsAsync(Guid assignmentId);
    }
}


