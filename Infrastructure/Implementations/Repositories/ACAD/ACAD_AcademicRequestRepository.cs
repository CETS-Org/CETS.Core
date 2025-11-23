using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_AcademicRequestRepository : BaseRepository<ACAD_AcademicRequest>, IACAD_AcademicRequestRepository
    {
        public ACAD_AcademicRequestRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<ACAD_AcademicRequest>> GetByStudentAsync(Guid studentId)
        {
            return await _context.ACAD_AcademicRequests
                .Include(r => r.Student)
                    .ThenInclude (s => s.Account)
                .Include(r => r.RequestType)
                .Include(r => r.AcademicRequestStatus)
                .Include(r => r.FromClass)
                .Include(r => r.ToClass)
                .Include(r => r.ProcessedByNavigation)
                .Include(r => r.ClassMeeting)
                    .ThenInclude(m => m.Slot)
                .Include(r => r.NewSlot)
                .Include(r => r.NewRoom)
                .Where(r => r.StudentID == studentId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ACAD_AcademicRequest>> GetByStatusAsync(Guid statusId)
        {
            return await _context.ACAD_AcademicRequests
                .Include(r => r.Student)
                    .ThenInclude(s => s.Account)
                .Include(r => r.RequestType)
                .Include(r => r.AcademicRequestStatus)
                .Include(r => r.FromClass)
                .Include(r => r.ToClass)
                .Include(r => r.ProcessedByNavigation)
                .Where(r => r.AcademicRequestStatusID == statusId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ACAD_AcademicRequest>> GetAllAsync()
        {
            return await _context.ACAD_AcademicRequests
                .Include(r => r.Student)
                    .ThenInclude(s => s.Account)
                .Include(r => r.RequestType)
                .Include(r => r.AcademicRequestStatus)
                .Include(r => r.FromClass)
                .Include(r => r.ToClass)
                .Include(r => r.ProcessedByNavigation)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<ACAD_AcademicRequest?> GetDetailsAsync(Guid requestId)
        {
            return await _context.ACAD_AcademicRequests
                .Include(r => r.Student)
                    .ThenInclude(s => s.Account)
                .Include(r => r.RequestType)
                .Include(r => r.AcademicRequestStatus)
                .Include(r => r.FromClass)
                .Include(r => r.ToClass)
                .Include(r => r.ProcessedByNavigation)
                .Include(r => r.ClassMeeting)
                    .ThenInclude(m => m.Slot)
                .Include(r => r.ClassMeeting)
                    .ThenInclude(m => m.Room)
                .Include(r => r.ClassMeeting)
                    .ThenInclude(m => m.CoveredTopic)
                .Include(r => r.NewSlot)
                .Include(r => r.NewRoom)
                .Include(r => r.ACAD_AcademicRequestHistories)
                    .ThenInclude(h => h.Status)
                .FirstOrDefaultAsync(r => r.Id == requestId);
        }
    }
}


