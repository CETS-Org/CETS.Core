using Application.Interfaces.ACAD;
using Domain.Entities.MongoDB;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_ExitSurvey.Requests;
using DTOs.ACAD.ACAD_ExitSurvey.Responses;

namespace Application.Implementations.ACAD;

public class ACAD_ExitSurveyService : IACAD_ExitSurveyService
{
    private readonly IACAD_ExitSurveyRepository _exitSurveyRepository;

    public ACAD_ExitSurveyService(IACAD_ExitSurveyRepository exitSurveyRepository)
    {
        _exitSurveyRepository = exitSurveyRepository;
    }

    public async Task<ExitSurveyResponse> CreateExitSurveyAsync(CreateExitSurveyRequest request)
    {
        var exitSurvey = new ACAD_ExitSurvey
        {
            StudentId = request.StudentId,
            AcademicRequestId = request.AcademicRequestId,
            ReasonCategory = request.ReasonCategory,
            ReasonDetail = request.ReasonDetail,
            Feedback = new ExitSurveyFeedback
            {
                TeacherQuality = request.Feedback.TeacherQuality,
                ClassPacing = request.Feedback.ClassPacing,
                Materials = request.Feedback.Materials,
                StaffService = request.Feedback.StaffService,
                Schedule = request.Feedback.Schedule,
                Facilities = request.Feedback.Facilities
            },
            FutureIntentions = new ExitSurveyFutureIntentions
            {
                WouldReturnInFuture = request.FutureIntentions.WouldReturnInFuture,
                WouldRecommendToOthers = request.FutureIntentions.WouldRecommendToOthers
            },
            Comments = request.Comments,
            AcknowledgesPermanent = request.AcknowledgesPermanent,
            CompletedAt = request.CompletedAt
        };

        var created = await _exitSurveyRepository.CreateAsync(exitSurvey);
        return MapToResponse(created);
    }

    public async Task<ExitSurveyResponse?> GetExitSurveyByIdAsync(string id)
    {
        var exitSurvey = await _exitSurveyRepository.GetByIdAsync(id);
        return exitSurvey != null ? MapToResponse(exitSurvey) : null;
    }

    public async Task<ExitSurveyResponse?> GetExitSurveyByAcademicRequestIdAsync(string academicRequestId)
    {
        var exitSurvey = await _exitSurveyRepository.GetByAcademicRequestIdAsync(academicRequestId);
        return exitSurvey != null ? MapToResponse(exitSurvey) : null;
    }

    public async Task<IReadOnlyList<ExitSurveyResponse>> GetExitSurveysByStudentAsync(string studentId)
    {
        var exitSurveys = await _exitSurveyRepository.GetByStudentAsync(studentId);
        return exitSurveys.Select(MapToResponse).ToList();
    }

    public async Task<IReadOnlyList<ExitSurveyResponse>> GetAllExitSurveysAsync()
    {
        var exitSurveys = await _exitSurveyRepository.GetAllAsync();
        return exitSurveys.Select(MapToResponse).ToList();
    }

    public async Task<ExitSurveyAnalyticsResponse> GetExitSurveyAnalyticsAsync()
    {
        var totalSurveys = await _exitSurveyRepository.GetTotalSurveysCountAsync();
        var reasonStats = await _exitSurveyRepository.GetReasonCategoryStatisticsAsync();
        var avgRatings = await _exitSurveyRepository.GetAverageFeedbackRatingsAsync();
        
        var now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1);
        var startOfYear = new DateTime(now.Year, 1, 1);
        
        var surveysThisMonth = await _exitSurveyRepository.GetSurveysCountByDateRangeAsync(startOfMonth, now);
        var surveysThisYear = await _exitSurveyRepository.GetSurveysCountByDateRangeAsync(startOfYear, now);

        return new ExitSurveyAnalyticsResponse
        {
            TotalSurveys = totalSurveys,
            ReasonCategoryStatistics = reasonStats,
            AverageFeedbackRatings = avgRatings,
            SurveysThisMonth = surveysThisMonth,
            SurveysThisYear = surveysThisYear
        };
    }

    public async Task<bool> DeleteExitSurveyAsync(string id)
    {
        await _exitSurveyRepository.DeleteAsync(id);
        return true;
    }

    private ExitSurveyResponse MapToResponse(ACAD_ExitSurvey exitSurvey)
    {
        return new ExitSurveyResponse
        {
            Id = exitSurvey.Id,
            StudentId = exitSurvey.StudentId,
            AcademicRequestId = exitSurvey.AcademicRequestId,
            ReasonCategory = exitSurvey.ReasonCategory,
            ReasonDetail = exitSurvey.ReasonDetail,
            Feedback = new DTOs.ACAD.ACAD_ExitSurvey.Responses.ExitSurveyFeedbackDto
            {
                TeacherQuality = exitSurvey.Feedback.TeacherQuality,
                ClassPacing = exitSurvey.Feedback.ClassPacing,
                Materials = exitSurvey.Feedback.Materials,
                StaffService = exitSurvey.Feedback.StaffService,
                Schedule = exitSurvey.Feedback.Schedule,
                Facilities = exitSurvey.Feedback.Facilities
            },
            FutureIntentions = new DTOs.ACAD.ACAD_ExitSurvey.Responses.ExitSurveyFutureIntentionsDto
            {
                WouldReturnInFuture = exitSurvey.FutureIntentions.WouldReturnInFuture,
                WouldRecommendToOthers = exitSurvey.FutureIntentions.WouldRecommendToOthers
            },
            Comments = exitSurvey.Comments,
            AcknowledgesPermanent = exitSurvey.AcknowledgesPermanent,
            CompletedAt = exitSurvey.CompletedAt,
            CreatedAt = exitSurvey.CreatedAt
        };
    }
}

