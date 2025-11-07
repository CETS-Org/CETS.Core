using DTOs.Analytics.ClassOverview.Requests;
using DTOs.Analytics.ClassOverview.Responses;

namespace Application.Interfaces.Analytics
{
    /// <summary>
    /// Service for Class-level analytics
    /// </summary>
    public interface IClassAnalyticsService
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



