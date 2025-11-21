using DTOs.ACAD.ACAD_Course.Responses;
using DTOs.ACAD.ACAD_CourseTeacherAssignment.Requests;
using DTOs.ACAD.ACAD_CourseTeacherAssignment.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_CourseTeacherAssignmentService
    {
        Task<IEnumerable<CourseListAssignmentResponse>> GetCoursesByTeacherIdAsync(Guid teacherId);
        Task<IEnumerable<ClassTeachingListResponse>?> GetTeachingClassesByTeacherIdAndCourseIdAsync(Guid teacherId, Guid courseId);
        Task<IEnumerable<TeachingCourseResponse>> GetAllTeachingCourses(Guid teacherId);
        Task<IEnumerable<CourseTeacherAssignmentResponse>> GetAssignmentsByCourseIdAsync(Guid courseId);
        Task<CourseTeacherAssignmentResponse> CreateAssignmentAsync(CreateCourseTeacherAssignmentRequest request);
        Task DeleteAssignmentAsync(Guid assignmentId);
    }
}