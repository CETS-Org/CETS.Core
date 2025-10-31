using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_ClassMeetings.Responses;
using DTOs.ACAD.ACAD_SyllabusItem.Responses;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_ClassMeetingRepository : BaseRepository<ACAD_ClassMeeting>, IACAD_ClassMeetingRepository
    {

        public ACAD_ClassMeetingRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ACAD_ClassMeeting>> GetAllClassMeetingByClassId(Guid classId)
        {
            return await _context.ACAD_ClassMeetings
                 .Where(cta => cta.ClassID == classId)
                 .Include(c => c.Room)
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

        public async Task<ACAD_SyllabusItem> GetCoveredTopicByClassMeetingId(Guid classMeetingId)
        {
            var meeting = await _context.ACAD_ClassMeetings
            .Include(cm => cm.CoveredTopic)
            .Where(cm => cm.Id == classMeetingId && !cm.IsDeleted)
            .Select(cm => cm.CoveredTopic)
            .FirstOrDefaultAsync();
            return meeting;
            
           
        }

        public async Task<IEnumerable<StudentWeeklyScheduleResponse>> WeeklyScheduleGetByStudentAsync(Guid studentId, CancellationToken ct)
        {
            var enrolled = await _context.ACAD_Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentID == studentId)
                .Select(e => new { e.ClassID, e.Course.CourseName })
                .ToListAsync(ct);

            if (!enrolled.Any())
                return Enumerable.Empty<StudentWeeklyScheduleResponse>();

            var classIds = enrolled.Select(e => e.ClassID).ToList();
            var courseMap = enrolled
                            .GroupBy(e => e.ClassID)
                            .ToDictionary(g => g.Key, g => g.First().CourseName);


            var meetings = await _context.ACAD_ClassMeetings
                .Include(m => m.Class)
                    .ThenInclude(m => m.TeacherAssignment)
                        .ThenInclude(ta => ta.Teacher.Account)
                .Include(m => m.Slot)
                .Include(m => m.Room)
                .Where(m => classIds.Contains(m.ClassID))
                .OrderBy(m => m.Date)
                .ThenBy(m => m.Slot.Name)
                .ToListAsync(ct);

            var result = meetings
            .Select(m =>
            {
                var startStr = m.Slot.Name?.Trim();
                string endStr = string.Empty;

                if (TimeSpan.TryParse(startStr, out var start))
                {
                    endStr = (start + TimeSpan.FromMinutes(90)).ToString(@"hh\:mm");
                }

                return new StudentWeeklyScheduleResponse
                {
                    Date = m.Date.ToDateTime(TimeOnly.MinValue),
                    DayOfWeek = m.Date.DayOfWeek,
                    Slot = m.Slot.Code,                
                    StartTime = startStr,              
                    EndTime = endStr,               
                    ClassName = m.Class.ClassName,
                    ClassId = m.ClassID.ToString(), // ✅ Added for navigation to class detail
                    CourseName = courseMap.ContainsKey(m.ClassID) ? courseMap[m.ClassID] : string.Empty,
                    Room = m.Room?.RoomCode,
                    Teacher = m.Class.TeacherAssignment?.Teacher.Account.FullName,
                    OnlineMeetingUrl = m.OnlineMeetingUrl
                };
            });
            return result.ToList();
        }

        public async Task<IEnumerable<TeacherWeeklyScheduleResponse>> WeeklyScheduleGetByTeacherAsync(Guid teacherId, CancellationToken ct)
        {
            // Lấy các lớp mà teacher được assign
            var teacherClasses = await _context.ACAD_Classes
                .Include(c => c.TeacherAssignment)
                    .ThenInclude(ta => ta.Course)
                .Where(c => c.TeacherAssignment.TeacherID == teacherId)
                .Select(c => new { c.Id, c.ClassName, c.Capacity, c.EnrolledCount, c.TeacherAssignment.Course.CourseName })
                .ToListAsync(ct);

            if (!teacherClasses.Any())
                return Enumerable.Empty<TeacherWeeklyScheduleResponse>();

            var classIds = teacherClasses.Select(c => c.Id).ToList();
            var classMap = teacherClasses.ToDictionary(c => c.Id, c => new { c.ClassName, c.CourseName, c.Capacity, c.EnrolledCount });

            var meetings = await _context.ACAD_ClassMeetings
                .Include(m => m.Class)
                .Include(m => m.Slot)
                .Include(m => m.Room)
                .Where(m => classIds.Contains(m.ClassID))
                .OrderBy(m => m.Date)
                .ThenBy(m => m.Slot.Name)
                .ToListAsync(ct);

            var result = meetings
            .Select(m =>
            {
                var startStr = m.Slot.Name?.Trim();
                string endStr = string.Empty;

                if (TimeSpan.TryParse(startStr, out var start))
                {
                    endStr = (start + TimeSpan.FromMinutes(90)).ToString(@"hh\:mm");
                }

                var classInfo = classMap.ContainsKey(m.ClassID) ? classMap[m.ClassID] : null;

                return new TeacherWeeklyScheduleResponse
                {
                    Date = m.Date.ToDateTime(TimeOnly.MinValue),
                    DayOfWeek = m.Date.DayOfWeek.ToString(),
                    Slot = m.Slot.Code,
                    StartTime = startStr,
                    EndTime = endStr,
                    ClassName = classInfo?.ClassName ?? string.Empty,
                    ClassId = m.ClassID.ToString(), // ✅ Added for navigation to class detail
                    CourseName = classInfo?.CourseName ?? string.Empty,
                    Room = m.Room?.RoomCode,
                    EnrolledCount = classInfo?.EnrolledCount ?? 0,
                    Capacity = classInfo?.Capacity ?? 0,
                    OnlineMeetingUrl = m.OnlineMeetingUrl
                };
            });
            return result.ToList();
        }

    }
}


