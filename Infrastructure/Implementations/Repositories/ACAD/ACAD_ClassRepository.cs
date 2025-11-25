using DocumentFormat.OpenXml.Office.CoverPageProps;
using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_Class.Responses;
using DTOs.ACAD.ACAD_ClassMeetings.Responses;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_ClassRepository : BaseRepository<ACAD_Class>, IACAD_ClassRepository
    {
        public ACAD_ClassRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<ACAD_Class>> GetAllClass()
        {
            return await _context.ACAD_Classes
                .Include(c => c.TeacherAssignment)
                    .ThenInclude(ta => ta.Teacher)
                        .ThenInclude(t => t.Account)
                .Include(c => c.ClassStatus)
                .Where(c => !c.IsDeleted)              
                .ToListAsync();
        }

        public async Task<List<LearningClassResponse>> GetLearningClassByStudentId(Guid studentId)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        /* public async Task<List<LearningClassResponse>> GetLearningClassByStudentId(Guid studentId)
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
                     TeacherId = assign != null ? assign.TeacherID : (Guid?)null,
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
                     CourseCode = e.Course.CourseCode,
                     ClassName = e.Class.ClassName,
                     TeacherId = e.TeacherId,
                     TeacherName = e.TeacherName,
                     StartDate = e.Class.StartDate,
                     EndDate = e.Class.EndDate,
                     TimeSlot = timeSlotName,
                     RoomCode = roomCode,
                     IsActive = e.Class.IsActive
                 });
             }

             return responses;
         }*/

       public async Task<List<LearningClassResponse>> GetLearningClassByStudentId(Guid studentId)
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);

                // 1 QUERY duy nhất, mọi thứ đều nằm trong projection
                var items = await (
                    from enroll in _context.ACAD_Enrollments
                    where enroll.StudentID == studentId && !enroll.IsDeleted

                    join cls in _context.ACAD_Classes on enroll.ClassID equals cls.Id
                    where !cls.IsDeleted

                    join course in _context.ACAD_Courses on enroll.CourseID equals course.Id
                    join statusLookup in _context.CORE_LookUps on cls.ClassStatusID equals statusLookup.Id

                    join assignOpt in _context.ACAD_CourseTeacherAssignments
                        on cls.TeacherAssignmentID equals assignOpt.Id into assignLeft
                    from assign in assignLeft.DefaultIfEmpty()

                    // Subquery: tìm buổi học sắp tới nhất của từng class + lấy luôn RoomCode, SlotName
                    select new
                    {
                        Class = cls,
                        Course = course,
                        StatusLookup = statusLookup,
                        TeacherId = assign != null ? assign.TeacherID : (Guid?)null,
                        TeacherName = assign != null ? assign.Teacher.Account.FullName : null,

                        NextMeeting = (
                            from m in _context.ACAD_ClassMeetings
                            where m.ClassID == cls.Id
                                  && !m.IsDeleted
                                  && m.IsActive
                                  && m.Date >= today
                            orderby m.Date
                            select new
                            {
                                m.Id,
                                m.ClassID,
                                m.Date,
                                m.IsStudy,
                                m.RoomID,
                                m.OnlineMeetingUrl,
                                m.Passcode,
                                m.RecordingUrl,
                                m.IsActive,
                                SlotName = m.Slot != null ? m.Slot.Name : null,
                                RoomCode = m.Room != null ? m.Room.RoomCode : null,
                                CoveredTopic = m.CoveredTopic != null ? m.CoveredTopic.TopicTitle : null
                            }
                        ).FirstOrDefault()
                    }
                ).ToListAsync();

                var responses = new List<LearningClassResponse>();

                foreach (var e in items)
                {
                    ClassMeetingResponse? nextMeetingDto = null;

                    if (e.NextMeeting != null)
                    {
                        nextMeetingDto = new ClassMeetingResponse
                        {
                            Id = e.NextMeeting.Id,
                            ClassID = e.NextMeeting.ClassID,
                            Date = e.NextMeeting.Date,
                            IsStudy = e.NextMeeting.IsStudy,
                            RoomID = e.NextMeeting.RoomID.ToString(),          // Guid? trong DTO
                            RoomCode = e.NextMeeting.RoomCode,
                            OnlineMeetingUrl = e.NextMeeting.OnlineMeetingUrl,
                            Passcode = e.NextMeeting.Passcode,
                            RecordingUrl = e.NextMeeting.RecordingUrl,
                            IsActive = e.NextMeeting.IsActive,
                            slot = e.NextMeeting.SlotName  ,
                            coveredTopic = e.NextMeeting.CoveredTopic// tên ca học
                        };
                    }

                    responses.Add(new LearningClassResponse
                    {
                        Id = e.Class.Id,
                        StatusName = e.StatusLookup.Name,
                        CourseName = e.Course.CourseName,
                        CourseCode = e.Course.CourseCode,
                        ClassName = e.Class.ClassName,
                        TeacherId = e.TeacherId,
                        TeacherName = e.TeacherName,
                        StartDate = e.Class.StartDate,
                        EndDate = e.Class.EndDate,
                        TimeSlot = e.NextMeeting?.SlotName,       // dùng dữ liệu từ subquery
                        RoomCode = e.NextMeeting?.RoomCode,
                        IsActive = e.Class.IsActive,
                        nextMeeting = nextMeetingDto
                    });
                }

                return responses;
            }


        public async Task<List<FeedbackClassResponse>> GetFeedbackClassesByStudentId(Guid studentId)
        {
            var enrollments = await _context.ACAD_Enrollments
                .Where(e => e.StudentID == studentId && !e.IsDeleted)
                .Include(e => e.Class)
                    .ThenInclude(c => c.TeacherAssignment)
                        .ThenInclude(ta => ta.Teacher)
                            .ThenInclude(t => t.Account)
                .Include(e => e.Course)
                .Where(e => e.Class != null && !e.Class.IsDeleted && e.Class.IsActive)
                .ToListAsync();

            var uniqueCourses = enrollments
                .GroupBy(e => e.CourseID)
                .Select(g => g.First())
                .ToList();

            var feedbackClasses = new List<FeedbackClassResponse>();

            foreach (var enrollment in uniqueCourses)
            {
                // Check if student has already submitted feedback for this course
                var hasSubmittedFeedback = await _context.COM_Feedbacks
                    .AnyAsync(f => f.SubmitterID == studentId 
                                && f.CourseID == enrollment.CourseID 
                                && !f.IsDeleted);

                feedbackClasses.Add(new FeedbackClassResponse
                {
                    CourseId = enrollment.Course.Id,
                    CourseName = enrollment.Course.CourseName,
                    TeacherId = enrollment.Class?.TeacherAssignment?.TeacherID,
                    TeacherName = enrollment.Class?.TeacherAssignment?.Teacher?.Account?.FullName,
                    HasSubmittedFeedback = hasSubmittedFeedback
                });
            }

            return feedbackClasses;
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

        public async Task<List<ClassResponse>> GetClassesByCourseIdAsync(Guid courseId)
        {
            var query = from cls in _context.ACAD_Classes
                        where !cls.IsDeleted
                        join assignOpt in _context.ACAD_CourseTeacherAssignments on cls.TeacherAssignmentID equals assignOpt.Id into assignLeft
                        from assign in assignLeft.DefaultIfEmpty()
                        where assign != null && assign.CourseID == courseId
                        join statusOpt in _context.CORE_LookUps on cls.ClassStatusID equals statusOpt.Id into statusLeft
                        from status in statusLeft.DefaultIfEmpty()
                        select new ClassResponse
                        {
                            Id = cls.Id,
                            ClassName = cls.ClassName ?? string.Empty,
                            StatusName = status != null ? status.Name : string.Empty,
                            StartDate = cls.StartDate,
                            EndDate = cls.EndDate,
                            Capacity = cls.Capacity,
                            EnrolledCount = cls.EnrolledCount,
                            IsActive = cls.IsActive
                        };

            return await query.ToListAsync();
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

        public async Task<List<ClassRowResponse>> GetAllClassRowsAsync()
        {
            var query = from cls in _context.ACAD_Classes
                        where !cls.IsDeleted
                        join assignOpt in _context.ACAD_CourseTeacherAssignments on cls.TeacherAssignmentID equals assignOpt.Id into assignLeft
                        from assign in assignLeft.DefaultIfEmpty()
                        join courseOpt in _context.ACAD_Courses on assign.CourseID equals courseOpt.Id into courseLeft
                        from course in courseLeft.DefaultIfEmpty()
                        join teacherOpt in _context.IDN_Teachers on assign.TeacherID equals teacherOpt.Id into teacherLeft
                        from teacher in teacherLeft.DefaultIfEmpty()
                        join accountOpt in _context.IDN_Accounts on teacher.Id equals accountOpt.Id into accountLeft
                        from account in accountLeft.DefaultIfEmpty()
                        join statusLookup in _context.CORE_LookUps on cls.ClassStatusID equals statusLookup.Id
                        select new
                        {
                            Class = cls,
                            Course = course,
                            TeacherName = account != null ? account.FullName : string.Empty,
                            StatusCode = statusLookup.Code,
                            StatusName = statusLookup.Name,
                            Meetings = _context.ACAD_ClassMeetings
                                .Where(m => m.ClassID == cls.Id && !m.IsDeleted && m.IsActive)
                                .OrderBy(m => m.Date)
                                .Select(m => new
                                {
                                    Date = m.Date,
                                    SlotCode = m.Slot.Code,
                                    RoomCode = m.Room != null ? m.Room.RoomCode : string.Empty
                                })
                                .ToList()
                        };

            var items = await query.ToListAsync();

            var responses = new List<ClassRowResponse>();

            foreach (var item in items)
            {
                // Determine status: if enrolled >= capacity then "full", else check IsActive
                string status;
                if (item.Class.EnrolledCount >= item.Class.Capacity)
                {
                    status = "full";
                }
                else if (item.Class.IsActive)
                {
                    status = "active";
                }
                else
                {
                    status = "inactive";
                }

                // Get room from the first meeting or most common room
                var room = item.Meetings.FirstOrDefault()?.RoomCode ?? string.Empty;

                // Build schedule
                var schedule = item.Meetings.Select(m => new ClassScheduleItem
                {
                    Date = m.Date.ToString("yyyy-MM-dd"),
                    Slot = m.SlotCode
                }).ToList();

                responses.Add(new ClassRowResponse
                {
                    Id = item.Class.Id.ToString(),
                    Name = item.Class.ClassName ?? string.Empty,
                    CourseId = item.Course?.Id.ToString() ?? string.Empty,
                    CourseName = item.Course?.CourseName ?? string.Empty,
                    Teacher = item.TeacherName,
                    Room = room,
                    CurrentStudents = item.Class.EnrolledCount,
                    MaxStudents = item.Class.Capacity,
                    Status = status,
                    Schedule = schedule.Any() ? schedule : null,
                    StartDate = item.Class.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = item.Class.EndDate.ToString("yyyy-MM-dd")
                });
            }
            return responses;
        }

        private IQueryable<ACAD_Class> StaffViewQuery()
            => _context.ACAD_Classes
                .AsNoTracking()
                .Include(c => c.TeacherAssignment)
                    .ThenInclude(ta => ta.Teacher)
                        .ThenInclude(t => t.Account)
                .Include(c => c.TeacherAssignment)
                    .ThenInclude(ta => ta.Course)              
                .Include(c => c.CourseFormat)
                .Include(c => c.ClassStatus)
              
                .Where(c => !c.IsDeleted);

        public async Task<List<ACAD_Class>> GetClassByCourseStaffView(Guid courseId)
        {
            return await StaffViewQuery()
               
                .Where(c => c.TeacherAssignment.Course.Id == courseId)
                .OrderByDescending(c => c.StartDate)
                .ToListAsync();
        }

        public async Task<ACAD_Class?> GetClassStaffViewById(Guid id)
        {
            return await StaffViewQuery()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

    }
}


