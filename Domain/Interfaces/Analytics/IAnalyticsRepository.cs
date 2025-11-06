using DTOs.Analytics.OverallOverview.Responses;

namespace Domain.Interfaces.Analytics
{
    public interface IAnalyticsRepository
    {
        // ===== NEW CATEGORY-BASED METHODS =====
        /// <summary>
        /// Get center performance metrics (room utilization, class operations, capacity)
        /// </summary>
        Task<CenterPerformanceResponse> GetCenterPerformanceAsync();

        /// <summary>
        /// Get growth and retention metrics (enrollment trends, churn, reactivation)
        /// </summary>
        Task<GrowthRetentionResponse> GetGrowthRetentionAsync();

        /// <summary>
        /// Get revenue and finance metrics (revenue trends, tuition, refunds, forecast)
        /// </summary>
        Task<RevenueFinanceResponse> GetRevenueFinanceAsync();

        /// <summary>
        /// Get engagement and satisfaction metrics (NPS, feedback, participation)
        /// </summary>
        Task<EngagementSatisfactionResponse> GetEngagementSatisfactionAsync();

        /// <summary>
        /// Get system health metrics (course utilization, teacher workload, system load)
        /// </summary>
        Task<SystemHealthResponse> GetSystemHealthAsync();

        /// <summary>
        /// Get overall overview with all category metrics
        /// </summary>
        Task<OverallOverviewResponse> GetOverallOverviewAsync();

        // ===== LEGACY METHODS (for backward compatibility) =====
        Task<StudentMetricsResponse> GetStudentMetricsAsync();

        Task<FinancialMetricsResponse> GetFinancialMetricsAsync();

        Task<CourseClassMetricsResponse> GetCourseClassMetricsAsync();

        Task<TeacherMetricsResponse> GetTeacherMetricsAsync();

        Task<EventMetricsResponse> GetEventMetricsAsync();

        Task<FeedbackMetricsResponse> GetFeedbackMetricsAsync();
    }
}



