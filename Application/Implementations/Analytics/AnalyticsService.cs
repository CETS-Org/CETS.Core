using Application.Interfaces.Analytics;
using Domain.Interfaces.Analytics;
using DTOs.Analytics.OverallOverview.Responses;

namespace Application.Implementations.Analytics
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IAnalyticsRepository _analyticsRepository;

        public AnalyticsService(IAnalyticsRepository analyticsRepository)
        {
            _analyticsRepository = analyticsRepository;
        }

        public async Task<StudentMetricsResponse> GetStudentMetricsAsync()
        {
            return await _analyticsRepository.GetStudentMetricsAsync();
        }

        public async Task<FinancialMetricsResponse> GetFinancialMetricsAsync()
        {
            return await _analyticsRepository.GetFinancialMetricsAsync();
        }

        public async Task<CourseClassMetricsResponse> GetCourseClassMetricsAsync()
        {
            return await _analyticsRepository.GetCourseClassMetricsAsync();
        }

        public async Task<TeacherMetricsResponse> GetTeacherMetricsAsync()
        {
            return await _analyticsRepository.GetTeacherMetricsAsync();
        }

        public async Task<EventMetricsResponse> GetEventMetricsAsync()
        {
            return await _analyticsRepository.GetEventMetricsAsync();
        }

        public async Task<FeedbackMetricsResponse> GetFeedbackMetricsAsync()
        {
            return await _analyticsRepository.GetFeedbackMetricsAsync();
        }

        public async Task<OverallOverviewResponse> GetOverallOverviewAsync()
        {
            return await _analyticsRepository.GetOverallOverviewAsync();
        }
    }
}





