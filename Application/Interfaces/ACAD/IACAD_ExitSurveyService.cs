using DTOs.ACAD.ACAD_ExitSurvey.Requests;
using DTOs.ACAD.ACAD_ExitSurvey.Responses;

namespace Application.Interfaces.ACAD;

public interface IACAD_ExitSurveyService
{
    Task<ExitSurveyResponse> CreateExitSurveyAsync(CreateExitSurveyRequest request);
    Task<ExitSurveyResponse?> GetExitSurveyByIdAsync(string id);
    Task<ExitSurveyResponse?> GetExitSurveyByAcademicRequestIdAsync(string academicRequestId);
    Task<IReadOnlyList<ExitSurveyResponse>> GetExitSurveysByStudentAsync(string studentId);
    Task<IReadOnlyList<ExitSurveyResponse>> GetAllExitSurveysAsync();
    Task<ExitSurveyAnalyticsResponse> GetExitSurveyAnalyticsAsync();
    Task<bool> DeleteExitSurveyAsync(string id);
}

