using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_ClassMeetingRepository : BaseRepository<ACAD_ClassMeeting>, IACAD_ClassMeetingRepository
    {
        public ACAD_ClassMeetingRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<ACAD_ClassMeeting>> GetAllClassMeetingByClassId(Guid classId)
        {
            return await _context.ACAD_ClassMeetings
                 .Where(cta => cta.ClassID == classId)
                 .Include(c => c.Slot)
                 .Include(c => c.Room)
                 .Include(c => c.CoveredTopic)
                 .ToListAsync();
        }

        public async Task<ACAD_ClassMeeting?> GetClassMeetingTodayByClassId(Guid classId)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return await _context.ACAD_ClassMeetings
                .Where(cta => cta.ClassID == classId && cta.Date == today)
                .Include(c => c.Slot)
                .Include(c => c.Room)
                .Include(c => c.CoveredTopic)
                .FirstOrDefaultAsync();
        }

       
    }
}


