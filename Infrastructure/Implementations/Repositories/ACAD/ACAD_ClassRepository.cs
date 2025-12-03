using DocumentFormat.OpenXml.Office.CoverPageProps;
using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Domain.Interfaces.CORE;
using DTOs.ACAD.ACAD_Class.Responses;
using DTOs.ACAD.ACAD_ClassMeetings.Responses;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_ClassRepository : BaseRepository<ACAD_Class>, IACAD_ClassRepository
    {
        private readonly ICORE_LookUpRepository _lookUpRepository;
        public ACAD_ClassRepository(AppDbContext context, ICORE_LookUpRepository lookUpRepository) : base(context)
        {
            _lookUpRepository = lookUpRepository;
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
            var waitingStatusId = await _lookUpRepository.GetByCodeAsync("EnrollmentStatus", "Enrolled");

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
            // Main class query with all related entities
            var classEntity = await _context.ACAD_Classes
                .AsNoTracking()
                .Include(c => c.ClassStatus)
                .Include(c => c.TeacherAssignment)
                    .ThenInclude(ta => ta.Course)
                .Include(c => c.TeacherAssignment)
                    .ThenInclude(ta => ta.Teacher)
                        .ThenInclude(t => t.Account)
                .Where(c => c.Id == classId && !c.IsDeleted)
                .FirstOrDefaultAsync();

            if (classEntity == null)
                return null;

            // Get meetings for schedule, room, and session count
            var meetings = await _context.ACAD_ClassMeetings
                .AsNoTracking()
                .Where(m => m.ClassID == classId && !m.IsDeleted && m.IsActive)
                .Include(m => m.Room)
                .Include(m => m.Slot)
                .OrderBy(m => m.Date)
                .ToListAsync();

            // Calculate schedule string (e.g., "Mon, Wed, Fri - 8:00 AM")
            var scheduleGroups = meetings
                .GroupBy(m => m.Slot.Name)
                .Select(g => new
                {
                    SlotName = g.Key,
                    Days = g.Select(m => m.Date.DayOfWeek.ToString().Substring(0, 3)).Distinct()
                })
                .ToList();

            var scheduleString = scheduleGroups.Any()
                ? string.Join(", ", scheduleGroups.Select(g => $"{string.Join(", ", g.Days)} - {g.SlotName}"))
                : string.Empty;

            // Get most common room
            var room = meetings
                .Where(m => m.Room != null)
                .GroupBy(m => m.Room!.RoomCode)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault() ?? string.Empty;

            // Calculate completed sessions (meetings in the past with IsStudy = true)
            var today = DateOnly.FromDateTime(DateTime.Now);
            var completedSessions = meetings.Count(m => m.Date < today && m.IsStudy);
            var totalSessions = meetings.Count();

            // Get students enrolled in this class with attendance and progress
            // Filter by enrollment status code "Enrolled"
            var enrollments = await _context.ACAD_Enrollments
                .AsNoTracking()
                
                .Include(e => e.Student)
                    .ThenInclude(s => s.Account)
                .Include(e => e.EnrollmentStatus)
                .Include(e => e.Course) // Include Course to get StandardScore for IsPass calculation
                .Where(e => e.ClassID == classId && !e.IsDeleted && e.EnrollmentStatus.Code == "Enrolled")
                .ToListAsync();

            var studentResponses = new List<StudentInClassResponse>();

            foreach (var enrollment in enrollments)
            {
                var student = enrollment.Student;
                var account = student.Account;

                // Calculate attendance rate (only for past meetings)
                var totalPastMeetings = meetings.Count(m => m.Date < today && m.IsStudy);
                var attendedPastMeetings = 0;

                if (totalPastMeetings > 0)
                {
                    attendedPastMeetings = await _context.ACAD_Attendances
                        .Where(a => a.StudentID == student.Id &&
                                    a.Meeting.ClassID == classId &&
                                    a.Meeting.Date < today &&
                                    a.Meeting.IsStudy &&
                                    a.AttendanceStatus.Code == "Present")
                        .CountAsync();
                }

                var attendanceRate = totalPastMeetings > 0 ? (decimal)attendedPastMeetings / totalPastMeetings * 100 : 0;

                // Calculate progress percentage based on attended meetings vs total study meetings
                var totalStudyMeetings = meetings.Count(m => m.IsStudy);
                var attendedStudyMeetings = 0;

                if (totalStudyMeetings > 0)
                {
                    attendedStudyMeetings = await _context.ACAD_Attendances
                        .Where(a => a.StudentID == student.Id &&
                                    a.Meeting.ClassID == classId &&
                                    a.Meeting.IsStudy &&
                                    a.AttendanceStatus.Code == "Present")
                        .CountAsync();
                }

                var progressPercentage = totalStudyMeetings > 0 ? (decimal)attendedStudyMeetings / totalStudyMeetings * 100 : 0;

                studentResponses.Add(new StudentInClassResponse
                {
                    Id = account.Id,
                    EnrollmentId = enrollment.Id,
                    StudentCode = student.StudentCode ?? string.Empty,
                    Name = account.FullName ?? string.Empty,
                    Email = account.Email ?? string.Empty,
                    Phone = account.PhoneNumber ?? string.Empty,
                    JoinDate = enrollment.CreatedAt.ToString("yyyy-MM-dd"),
                    AttendanceRate = Math.Round(attendanceRate, 0),
                    ProgressPercentage = Math.Round(progressPercentage, 0),
                    FinalGrade = enrollment.FinalGrade,
                    IsPass = enrollment.IsPass // Read from database
                });
            }

            // Determine status
            string statusForDisplay;
            if (classEntity.EnrolledCount >= classEntity.Capacity)
            {
                statusForDisplay = "full";
            }
            else if (classEntity.IsActive)
            {
                statusForDisplay = "active";
            }
            else
            {
                statusForDisplay = "inactive";
            }

            // Extract related data
            var course = classEntity.TeacherAssignment?.Course;
            var teacher = classEntity.TeacherAssignment?.Teacher;
            var teacherAccount = teacher?.Account;

            return new ClassDetailResponse
            {
                Id = classEntity.Id,
                ClassName = classEntity.ClassName ?? string.Empty,
                CourseName = course?.CourseName ?? string.Empty,
                CourseId = course?.Id ?? Guid.Empty,
                Capacity = classEntity.Capacity,
                EnrolledCount = classEntity.EnrolledCount,
                TeacherId = teacher?.Id,
                TeacherName = teacherAccount?.FullName ?? string.Empty,
                Schedule = scheduleString,
                Room = room,
                StartDate = classEntity.StartDate.ToString("yyyy-MM-dd"),
                EndDate = classEntity.EndDate.ToString("yyyy-MM-dd"),
                Status = statusForDisplay,
                StatusCode = classEntity.ClassStatus?.Code ?? string.Empty,
                Description = course?.Description,
                TotalSessions = totalSessions,
                CompletedSessions = completedSessions,
                Students = studentResponses
            };
        }

        public async Task<List<ClassResponse>> GetClassesByCourseIdAsync(Guid courseId)
        {
            var query = from cls in _context.ACAD_Classes
                        where !cls.IsDeleted
                        join assignOpt in _context.ACAD_CourseTeacherAssignments on cls.TeacherAssignmentID equals assignOpt.Id into assignLeft
                        from assign in assignLeft.DefaultIfEmpty()
                            //where assign != null && assign.CourseID == courseId
                        where assign == null || assign.CourseID == courseId
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

        public async Task<List<ClassResponse>> GetClassesByCourseIdAsync2(Guid courseId)
        {
            var classes = await _context.ACAD_Classes
                .AsNoTracking()
                .Where(c => !c.IsDeleted)

                // Join TeacherAssignment (optional)
                .Include(c => c.TeacherAssignment)
                    .ThenInclude(ta => ta.Course)

                // Join Status lookup
                .Include(c => c.ClassStatus)

                // Chỉ lấy class có TeacherAssignment đúng course
                .Where(c => c.TeacherAssignment != null &&
                            c.TeacherAssignment.CourseID == courseId)

                .ToListAsync();

            // Map sang DTO
            return classes.Select(c => new ClassResponse
            {
                Id = c.Id,
                ClassName = c.ClassName ?? string.Empty,

                StatusName = c.ClassStatus?.Name ?? string.Empty,

                StartDate = c.StartDate,
                EndDate = c.EndDate,

                Capacity = c.Capacity,
                EnrolledCount = c.EnrolledCount,

                IsActive = c.IsActive
            }).ToList();
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

        public async Task<ACAD_Class?> GetClassWithDetailForEditAsync(Guid classId)
        {
            return await _context.ACAD_Classes
                .AsNoTracking() // Read-only để tối ưu hiệu năng khi get
                .Include(c => c.ACAD_ClassMeetings) // Lấy lịch học
                .Include(c => c.ACAD_Enrollments)   // Lấy danh sách enrollment
                    .ThenInclude(e => e.Student)    // Lấy thông tin học sinh
                        .ThenInclude(s => s.Account) // Lấy email/sđt
                .Include(c => c.TeacherAssignment)
                    .ThenInclude(ta => ta.Course)
                .Include(c => c.TeacherAssignment)
                    .ThenInclude(ta => ta.Teacher)
                        .ThenInclude(t => t.Account)
                
                .FirstOrDefaultAsync(c => c.Id == classId && !c.IsDeleted);
        }

    }
}


