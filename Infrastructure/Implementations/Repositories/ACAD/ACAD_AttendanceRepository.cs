using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.ACAD
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

        public async Task<int> CountTotalMeetingsByCourseAsync(Guid courseId)
        {
            return await _context.ACAD_ClassMeetings
                .Where(m => m.TeacherAssignment != null &&
                            m.TeacherAssignment.CourseID == courseId &&
                            m.StartsAt <= DateTime.UtcNow)
                .CountAsync();
        }

        public async Task<List<ACAD_Attendance>> GetByStudentAndCourseAsync(Guid studentId, Guid courseId)
        {
            return await _context.ACAD_Attendances
                .Include(a => a.AttendanceStatus)
                .Include(a => a.Meeting)
                    .ThenInclude(m => m.TeacherAssignment)
                .Where(a => a.StudentID == studentId &&
                            a.Meeting.TeacherAssignment != null &&
                            a.Meeting.TeacherAssignment.CourseID == courseId)
                .ToListAsync();
        }
    }
}


