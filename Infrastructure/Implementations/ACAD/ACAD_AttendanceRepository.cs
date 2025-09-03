using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_AttendanceRepository : BaseRepository<ACAD_Attendance>, IACAD_AttendanceRepository
    {
        public ACAD_AttendanceRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<ACAD_Attendance>> GetByMeetingAsync(Guid meetingId)
        {
            return await _context.ACAD_Attendances
                .Where(a => a.MeetingID == meetingId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ACAD_Attendance>> GetByStudentAsync(Guid studentId)
        {
            return await _context.ACAD_Attendances
                .Where(a => a.StudentID == studentId)
                .ToListAsync();
        }

        public async Task<ACAD_Attendance?> GetByMeetingAndStudentAsync(Guid meetingId, Guid studentId)
        {
            return await _context.ACAD_Attendances
                .FirstOrDefaultAsync(a => a.MeetingID == meetingId && a.StudentID == studentId);
        }
    }
}


