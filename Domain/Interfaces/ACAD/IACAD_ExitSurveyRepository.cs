using Domain.Entities.MongoDB;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_ExitSurveyRepository
    {
        Task<IReadOnlyList<ACAD_ExitSurvey>> GetAllAsync();
        Task<IReadOnlyList<ACAD_ExitSurvey>> GetByStudentAsync(string studentId);
        Task<ACAD_ExitSurvey?> GetByIdAsync(string id);
        Task<ACAD_ExitSurvey?> GetByAcademicRequestIdAsync(string academicRequestId);
        Task<ACAD_ExitSurvey> CreateAsync(ACAD_ExitSurvey document);
        Task<bool> UpdateAsync(ACAD_ExitSurvey document);
        Task DeleteAsync(string id);
        
        // Analytics methods
        Task<Dictionary<string, int>> GetReasonCategoryStatisticsAsync();
        Task<Dictionary<string, double>> GetAverageFeedbackRatingsAsync();
        Task<int> GetTotalSurveysCountAsync();
        Task<int> GetSurveysCountByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}

