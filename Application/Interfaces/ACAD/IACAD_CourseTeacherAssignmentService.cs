using DTOs.ACAD.ACAD_Course.Responses;
using DTOs.ACAD.ACAD_CourseTeacherAssignment.Request;
using DTOs.ACAD.ACAD_CourseTeacherAssignment.Requests;
using DTOs.ACAD.ACAD_CourseTeacherAssignment.Responses;
using DTOs.IDN.IDN_Teacher.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_CourseTeacherAssignmentService
    {
        Task<IEnumerable<CourseListAssignmentResponse>> GetCoursesByTeacherIdAsync(Guid teacherId);
        Task<IEnumerable<ClassTeachingListResponse>?> GetTeachingClassesByTeacherIdAndCourseIdAsync(Guid teacherId, Guid courseId);
        Task<IEnumerable<TeachingCourseResponse>> GetAllTeachingCourses(Guid teacherId);
        Task<IEnumerable<TeacherResponse>> GetTeachersByCourseAsync(Guid courseId);
        Task<IEnumerable<TeacherOptionResponse>> GetAvailableTeachersAsync(GetAvailableTeachersRequest request);
        Task<IEnumerable<CourseTeacherAssignmentResponse>> GetTeacherAssignmentByCourseAsync(Guid courseId);
        Task<IEnumerable<CourseTeacherAssignmentResponse>> GetAssignmentsByCourseIdAsync(Guid courseId);
        Task<CourseTeacherAssignmentResponse> CreateAssignmentAsync(CreateCourseTeacherAssignmentRequest request);
        Task DeleteAssignmentAsync(Guid assignmentId);
    }
}