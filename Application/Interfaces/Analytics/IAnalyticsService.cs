using DTOs.Analytics.OverallOverview.Responses;

namespace Application.Interfaces.Analytics
{
    public interface IAnalyticsService
    {
        /// <summary>
        /// Get student metrics for overall overview
        /// </summary>
        Task<StudentMetricsResponse> GetStudentMetricsAsync();

        /// <summary>
        /// Get financial metrics for overall overview
        /// </summary>
        Task<FinancialMetricsResponse> GetFinancialMetricsAsync();

        /// <summary>
        /// Get course and class metrics for overall overview
        /// </summary>
        Task<CourseClassMetricsResponse> GetCourseClassMetricsAsync();

        /// <summary>
        /// Get teacher metrics for overall overview
        /// </summary>
        Task<TeacherMetricsResponse> GetTeacherMetricsAsync();

        /// <summary>
        /// Get event metrics for overall overview
        /// </summary>
        Task<EventMetricsResponse> GetEventMetricsAsync();

        /// <summary>
        /// Get feedback and satisfaction metrics for overall overview
        /// </summary>
        Task<FeedbackMetricsResponse> GetFeedbackMetricsAsync();

        /// <summary>
        /// Get all overall overview metrics in one call
        /// </summary>
        Task<OverallOverviewResponse> GetOverallOverviewAsync();
    }
}



















