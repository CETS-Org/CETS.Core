using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_Class.Responses;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_ClassRepository : BaseRepository<ACAD_Class>, IACAD_ClassRepository
    {
        public ACAD_ClassRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<LearningClassResponse>> GetLearningClassByStudentId(Guid studentId)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);

            var items = await (
                from enroll in _context.ACAD_Enrollments
                where enroll.StudentID == studentId && !enroll.IsDeleted
                join cls in _context.ACAD_Classes on enroll.ClassID equals cls.Id
                where !cls.IsDeleted
                join course in _context.ACAD_Courses on enroll.CourseID equals course.Id
                join statusLookup in _context.CORE_LookUps on cls.ClassStatusID equals statusLookup.Id
                join assignOpt in _context.ACAD_CourseTeacherAssignments on cls.TeacherAssignmentID equals assignOpt.Id into assignLeft
                from assign in assignLeft.DefaultIfEmpty()
                select new
                {
                    Class = cls,
                    Course = course,
                    StatusLookup = statusLookup,
                    TeacherName = assign != null ? assign.Teacher.Account.FullName : null,
                    NextMeeting = _context.ACAD_ClassMeetings
                        .Where(m => m.ClassID == cls.Id && !m.IsDeleted && m.IsActive && m.Date >= today)
                        .OrderBy(m => m.Date)
                        .FirstOrDefault(),
                }
            ).ToListAsync();

            var responses = new List<LearningClassResponse>();

            foreach (var e in items)
            {
                var timeSlotName = e.NextMeeting != null
                    ? _context.CORE_LookUps.Where(l => l.Id == e.NextMeeting.SlotID).Select(l => l.Name).FirstOrDefault()
                    : null;

                var roomCode = e.NextMeeting != null && e.NextMeeting.RoomID != null
                    ? _context.FAC_Rooms.Where(r => r.Id == e.NextMeeting.RoomID).Select(r => r.RoomCode).FirstOrDefault()
                    : null;

                responses.Add(new LearningClassResponse
                {
                    Id = e.Class.Id,
                    StatusName = e.StatusLookup.Name,
                    CourseName = e.Course.CourseName,
                    ClassName = e.Class.ClassName,
                    TeacherName = e.TeacherName,
                    StartDate = e.Class.StartDate,
                    EndDate = e.Class.EndDate,
                    TimeSlot = timeSlotName,
                    RoomCode = roomCode,
                    IsActive = e.Class.IsActive
                });
            }

            return responses;
        }

        public async Task<ClassDetailResponse?> GetClassDetailAsync(Guid classId)
        {
            var query = from cls in _context.ACAD_Classes
                        where cls.Id == classId && !cls.IsDeleted
                        join assignOpt in _context.ACAD_CourseTeacherAssignments on cls.TeacherAssignmentID equals assignOpt.Id into assignLeft
                        from assign in assignLeft.DefaultIfEmpty()
                        join courseOpt in _context.ACAD_Courses on assign.CourseID equals courseOpt.Id into courseLeft
                        from course in courseLeft.DefaultIfEmpty()
                        select new ClassDetailResponse
                        {
                            Id = cls.Id,
                            ClassName = cls.ClassName.ToString(),
                            CourseName = course != null ? course.CourseName : string.Empty,
                            CourseId = course != null ? course.Id : Guid.Empty,
                            Capacity = cls.Capacity,
                            EnrolledCount = cls.EnrolledCount
                        };

            return await query.FirstOrDefaultAsync();
        }

        /*private async Task<string?> GetRoomByClassIdAsync(Guid? classId)
        {
            if (classId == null)
                return null;

            return await _context.ACAD_ClassMeetings
                .Where(c => c.Class.Id == classId)
                .Select(c => c.Room.RoomCode)  
                .FirstOrDefaultAsync();
        }*/
    }
}


