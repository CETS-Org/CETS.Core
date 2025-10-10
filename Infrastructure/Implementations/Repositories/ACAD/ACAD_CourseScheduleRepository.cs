using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_CourseScheduleRepository : BaseRepository<ACAD_CourseSchedule>, IACAD_CourseScheduleRepository
    {
        public ACAD_CourseScheduleRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ACAD_CourseSchedule>> GetSchedulesByCourseIdAsync(Guid courseId)
        {
            return await _context.Set<ACAD_CourseSchedule>()
                .Where(cs => cs.CourseID == courseId)
                .Include(cs => cs.Course)
                .Include(cs => cs.TimeSlot)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<ACAD_CourseSchedule>> GetSchedulesByDayOfWeekAsync(DayOfWeek dayOfWeek)
        {
            return await _context.Set<ACAD_CourseSchedule>()
                .Where(cs => cs.DayOfWeek == dayOfWeek)
                .Include(cs => cs.Course)
                .Include(cs => cs.TimeSlot)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<ACAD_CourseSchedule>> GetSchedulesByTimeSlotIdAsync(Guid timeSlotId)
        {
            return await _context.Set<ACAD_CourseSchedule>()
                .Where(cs => cs.TimeSlotID == timeSlotId)
                .Include(cs => cs.Course)
                .Include(cs => cs.TimeSlot)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> IsTimeSlotAvailableAsync(Guid courseId, Guid timeSlotId, DayOfWeek dayOfWeek)
        {
            return !await _context.Set<ACAD_CourseSchedule>()
                .AnyAsync(cs => cs.CourseID == courseId && 
                               cs.TimeSlotID == timeSlotId && 
                               cs.DayOfWeek == dayOfWeek);
        }

        public async Task<ACAD_CourseSchedule?> GetDetailByIdAsync(Guid id)
        {
            return await _context.Set<ACAD_CourseSchedule>()
                .Include(cs => cs.Course)
                .Include(cs => cs.TimeSlot)
                .AsNoTracking()
                .FirstOrDefaultAsync(cs => cs.Id == id);
        }

        public async Task<IEnumerable<ACAD_CourseSchedule>> GetAllWithNavigationPropertiesAsync()
        {
            return await _context.Set<ACAD_CourseSchedule>()
                .Include(cs => cs.Course)
                .Include(cs => cs.TimeSlot)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
