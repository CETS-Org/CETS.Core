using DTOs.Analytics.Dashboard.Requests;
using DTOs.Analytics.Dashboard.Responses;

namespace Application.Interfaces.Analytics;

/// <summary>
/// Service for dashboard analytics and insights
/// </summary>
public interface IDashboardAnalyticsService
{
    Task<RevenueAnalyticsResponse> GetRevenueAnalyticsAsync();
    
    Task<CourseEnrollmentStatsResponse> GetTopEnrolledCoursesAsync(TopCoursesRequest request);
    
    Task<StudentDropoutAnalyticsResponse> GetStudentDropoutAnalyticsAsync(DropoutAnalysisRequest request);
    
    Task<StudentEnrollmentAnalyticsResponse> GetEnrollmentAnalyticsAsync();
    
    Task<AIAnalysisResponse> GetAIRecommendationsAsync(AIRecommendationRequest request);
}


