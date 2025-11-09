using Domain.Entities;

namespace Domain.Interfaces.RPT
{
    public interface IRPT_ReportRepository : IBaseRepository<RPT_Report>
    {
      
        Task<IReadOnlyList<RPT_Report>> GetAcademicRequestsBySubmitterAsync(Guid submitterId);

        Task<IReadOnlyList<RPT_Report>> GetAcademicRequestsByCourseAsync(Guid courseId);
     
        Task<IReadOnlyList<RPT_Report>> GetAcademicRequestsByClassAsync(Guid classId);
   
        Task<IReadOnlyList<RPT_Report>> GetPendingAcademicRequestsAsync();

        Task<RPT_Report?> GetAcademicRequestWithDetailsAsync(Guid id);
    }
}


