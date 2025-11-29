using Domain.Entities;

namespace Domain.Interfaces.RPT
{
    public interface IRPT_ReportRepository : IBaseRepository<RPT_Report>
    {
        // Override GetAllAsync to include navigation properties
        new Task<IReadOnlyList<RPT_Report>> GetAllAsync();
      
        Task<IReadOnlyList<RPT_Report>> GetAcademicRequestsBySubmitterAsync(Guid submitterId);

        Task<IReadOnlyList<RPT_Report>> GetAcademicRequestsByCourseAsync(Guid courseId);
     
        Task<IReadOnlyList<RPT_Report>> GetAcademicRequestsByClassAsync(Guid classId);
   
        Task<IReadOnlyList<RPT_Report>> GetPendingAcademicRequestsAsync();

        Task<RPT_Report?> GetAcademicRequestWithDetailsAsync(Guid id);
        
        // System Complaint methods
        Task<IReadOnlyList<RPT_Report>> GetSystemComplaintsByReportTypeAsync(Guid reportTypeId);
        Task<IReadOnlyList<RPT_Report>> GetSystemComplaintsByStatusAsync(Guid reportTypeId, Guid statusId);
        Task<RPT_Report?> GetSystemComplaintWithDetailsAsync(Guid id);
    }
}


