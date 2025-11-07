using Application.Interfaces.Analytics;
using Domain.Interfaces.Analytics;
using DTOs.Analytics.ClassOverview.Requests;
using DTOs.Analytics.ClassOverview.Responses;

namespace Application.Implementations.Analytics
{
    public class ClassAnalyticsService : IClassAnalyticsService
    {
        private readonly IClassAnalyticsRepository _repository;

        public ClassAnalyticsService(IClassAnalyticsRepository repository)
        {
            _repository = repository;
        }

        public async Task<ClassOverviewResponse?> GetClassOverviewAsync(Guid classId)
        {
            return await _repository.GetClassOverviewAsync(classId);
        }

        public async Task<ClassListResponse> GetAllClassesOverviewAsync(ClassFilterRequest filter)
        {
            return await _repository.GetAllClassesOverviewAsync(filter);
        }

        public async Task<ClassSummaryResponse?> GetClassSummaryAsync(Guid classId)
        {
            return await _repository.GetClassSummaryAsync(classId);
        }
    }
}



