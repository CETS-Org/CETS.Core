using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.RPT;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.RPT
{
    public class RPT_ReportRepository : BaseRepository<RPT_Report>, IRPT_ReportRepository
    {
        public RPT_ReportRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<RPT_Report>> GetAcademicRequestsBySubmitterAsync(Guid submitterId)
        {
            return await _context.RPT_Reports
                .Include(r => r.ReportType)
                .Include(r => r.ReportStatus)
                .Include(r => r.SubmittedByNavigation)
                .Include(r => r.ResolvedByNavigation)
                .Where(r => r.SubmittedBy == submitterId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<RPT_Report>> GetAcademicRequestsByCourseAsync(Guid courseId)
        {
            // Note: Course/Class filtering removed as these fields no longer exist
            // This method returns empty list as course-based filtering is no longer supported
            return new List<RPT_Report>().AsReadOnly();
        }

        public async Task<IReadOnlyList<RPT_Report>> GetAcademicRequestsByClassAsync(Guid classId)
        {
            // Note: Course/Class filtering removed as these fields no longer exist
            // This method returns empty list as class-based filtering is no longer supported
            return new List<RPT_Report>().AsReadOnly();
        }

        public async Task<IReadOnlyList<RPT_Report>> GetPendingAcademicRequestsAsync()
        {
            return await _context.RPT_Reports
                .Include(r => r.ReportType)
                .Include(r => r.ReportStatus)
                .Include(r => r.SubmittedByNavigation)
                .Include(r => r.ResolvedByNavigation)
                .Where(r => r.ReportStatus.Code == "PENDING")
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<RPT_Report?> GetAcademicRequestWithDetailsAsync(Guid id)
        {
            return await _context.RPT_Reports
                .Include(r => r.ReportType)
                .Include(r => r.ReportStatus)
                .Include(r => r.SubmittedByNavigation)
                    .ThenInclude(a => a.IDN_StudentAccount)
                .Include(r => r.SubmittedByNavigation)
                    .ThenInclude(a => a.IDN_TeacherAccount)
                .Include(r => r.ResolvedByNavigation)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}


