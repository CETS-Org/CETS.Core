using Domain.Entities;
using DTOs.ACAD.ACAD_Submission.Requests;
using DTOs.ACAD.ACAD_Submission.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_SubmissionService
    {
        Task<SubmitAssignmentRequest> SubmitAssignmentAsync(SubmitAssignmentRequest request);
        Task<IEnumerable<SubmissionResponse>> GetSubmissionsByAssignmentAsync(Guid assignmentId);
        Task<IEnumerable<SubmissionResponse>> GetSubmissionsByStudentAsync(Guid studentId);
        Task<SubmissionResponse> GradeSubmissionAsync(GradeSubmissionRequest request);
        Task<(int submitted, int total)> GetAssignmentsSubmittedSummaryAsync(Guid studentId, Guid courseId);
        Task<SubmissionResponse> UpdateScoreAsync(UpdateSubmissionScoreRequest request);
        Task<SubmissionResponse> UpdateFeedbackAsync(UpdateSubmissionFeedbackRequest request);
        Task<string> GetDownloadUrlAsync(Guid id);
        Task<SubmissionResponse>GetSubmissionByIdAsync(Guid id);
        Task<AssignmentSubmissionsResponse> GetSubmissionsWithDownloadUrlsAsync(Guid assignmentId);
        Task<BulkUpdateSubmissionsResponse> BulkUpdateSubmissionsAsync(BulkUpdateSubmissionsRequest request);
    }

}
