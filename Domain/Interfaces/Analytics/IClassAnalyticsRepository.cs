using DTOs.Analytics.ClassOverview.Requests;
using DTOs.Analytics.ClassOverview.Responses;

namespace Domain.Interfaces.Analytics
{
    /// <summary>
    /// Repository for Class-level analytics
    /// </summary>
    public interface IClassAnalyticsRepository
    {
        /// <summary>
        /// Get comprehensive analytics for a specific class
        /// </summary>
        Task<ClassOverviewResponse?> GetClassOverviewAsync(Guid classId);

        /// <summary>
        /// Get summary list of all classes with basic metrics
        /// </summary>
        Task<ClassListResponse> GetAllClassesOverviewAsync(ClassFilterRequest filter);

        /// <summary>
        /// Get class summary by class ID
        /// </summary>
        Task<ClassSummaryResponse?> GetClassSummaryAsync(Guid classId);
    }
}



