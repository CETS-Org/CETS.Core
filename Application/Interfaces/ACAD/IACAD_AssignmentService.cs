using Domain.Entities;
using DTOs.ACAD.ACAD_Assignment.Requests;
using DTOs.ACAD.ACAD_Assignment.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_AssignmentService
    {
        Task<AssignmentResponse> CreateAssignmentAsync(CreateAssignmentRequest request);

        Task<AssignmentUploadResponse> CreateAssignmentWithFileAsync(CreateAssignmentWithFileRequest request);

        Task<QuizAssignmentResponse> CreateQuizAssignmentAsync(CreateQuizAssignmentRequest request);

        Task<SpeakingAssignmentResponse> CreateSpeakingAssignmentAsync(CreateSpeakingAssignmentRequest request);

        Task<IEnumerable<AssignmentResponse>> GetAssignmentsByClassMeetingAsync(Guid classMeetingId);

        Task<IEnumerable<AssignmentResponse>> GetAssignmentsByTeacherAsync(Guid teacherId);

        Task<AssignmentResponse?> GetAssignmentByIdAsync(Guid id);

        Task<AssignmentResponse> UpdateAssignmentAsync(UpdateAssignmentRequest request);

        Task DeleteAssignmentAsync(Guid id);
        Task<IEnumerable<AssignmentResponse>> GetAssignmentsWithSubmissions(Guid classMeetingId, Guid studentId);
        Task<IEnumerable<AssignmentWithSubmissionCountResponse>> GetAssignmentsWithSubmissionCountAsync(Guid classMeetingId);
        Task<string> GetDownloadUrlAsync(Guid id);
    }
}
