using Domain.Constants;
using Domain.Entities;
using DTOs.ACAD.ACAD_Course.Responses;
using DTOs.ACAD.ACAD_Enrollment.Requests;
using DTOs.ACAD.ACAD_Enrollment.Responses;
using DTOs.IDN.IDN_Student.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_EnrollmentService
    {
        Task<EnrollmentResponse> EnrollAsync(CreateEnrollmentRequest request);
        Task<IEnumerable<EnrollmentResponse>> GetStudentEnrollmentsAsync(Guid studentId);
        Task<IEnumerable<EnrollmentResponse>> GetClassEnrollmentsAsync(Guid classId);
        Task<EnrollmentDetailResponse?> GetEnrollmentDetailAsync(Guid enrollmentId);
        Task<IEnumerable<CourseEnrollmentListResponse>> GetStudentCoursesEnrollmentAsync(Guid studentId);
        Task<AcademicResultResponse> GetStudentAcademicResultsAsync(Guid studentId);
        Task<StudentCourseDetailResponse?> GetStudentCourseDetailAsync(Guid studentId, Guid courseId);
        Task<LearningPathOverviewResponse?> GetLearningPathOverviewAsync(Guid studentId);
        Task<WaitingStudentSearchResult> GetStudentWaitListAsync(Guid courseId, string? query, int page, int pageSize);
        Task<BulkUpdateFinalGradesResponse> BulkUpdateFinalGradesAsync(BulkUpdateFinalGradesRequest request);
        Task<EnrollmentForRefundResponse?> GetEnrollmentForRefund(Guid enrollmentId);
        Task<EmailDecisionStatus?> GetDecisionStatusAsync(Guid enrollmentId);
        Task UpdateDecisionStatusAsync(Guid enrollmentId, EmailDecisionStatus status);

    }
}
