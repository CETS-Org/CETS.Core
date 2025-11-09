using Domain.Data;
using Domain.Interfaces.Analytics;
using DTOs.Analytics.ClassOverview.Requests;
using DTOs.Analytics.ClassOverview.Responses;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.Analytics
{
    public class ClassAnalyticsRepository : IClassAnalyticsRepository
    {
        private readonly AppDbContext _context;

        public ClassAnalyticsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ClassOverviewResponse?> GetClassOverviewAsync(Guid classId)
        {
            var now = DateTime.Now;
            var today = DateOnly.FromDateTime(now);

            // Get class with related data
            var classEntity = await _context.ACAD_Classes
                .Include(c => c.ClassStatus)
                .Include(c => c.TeacherAssignment)
                    .ThenInclude(t => t!.Teacher)
                        .ThenInclude(t => t!.Account)
                .Include(c => c.ACAD_Enrollments)
                    .ThenInclude(e => e.Student)
                        .ThenInclude(s => s.Account)
                .Include(c => c.ACAD_Enrollments)
                    .ThenInclude(e => e.Course)
                .Include(c => c.ACAD_ClassMeetings)
                .FirstOrDefaultAsync(c => c.Id == classId && !c.IsDeleted);

            if (classEntity == null) return null;

            // Get course info
            var course = classEntity.ACAD_Enrollments.FirstOrDefault()?.Course;
            var courseId = course?.Id ?? Guid.Empty;
            var courseName = course?.CourseName ?? "Unknown";

            // Get enrollments
            var enrollments = classEntity.ACAD_Enrollments.Where(e => !e.IsDeleted).ToList();
            var studentIds = enrollments.Select(e => e.StudentID).ToList();

            // Get enrollment statuses
            var activeStatus = await _context.CORE_LookUps
                .Where(l => l.LookUpType.Code == "EnrollmentStatus" && 
                           (l.Code == "Active" || l.Code == "InProgress"))
                .Select(l => l.Id)
                .ToListAsync();

            var completedStatus = await _context.CORE_LookUps
                .Where(l => l.LookUpType.Code == "EnrollmentStatus" && l.Code == "Completed")
                .Select(l => l.Id)
                .ToListAsync();

            var droppedStatus = await _context.CORE_LookUps
                .Where(l => l.LookUpType.Code == "EnrollmentStatus" && 
                           (l.Code == "Dropped" || l.Code == "Cancelled"))
                .Select(l => l.Id)
                .ToListAsync();

            // 1. ATTENDANCE & ACTIVITY METRICS
            var attendanceActivity = await GetAttendanceActivityMetricsAsync(classId, studentIds, classEntity);

            // 2. PERFORMANCE METRICS
            var performance = await GetPerformanceMetricsAsync(classId, studentIds, enrollments);

            // 3. ENGAGEMENT METRICS
            var engagement = await GetEngagementMetricsAsync(classId, studentIds);

            // 4. OPERATIONAL METRICS
            var operational = GetOperationalMetrics(classEntity, today);

            // 5. TEACHER EFFECTIVENESS METRICS
            var teacherEffectiveness = await GetTeacherEffectivenessMetricsAsync(classEntity, studentIds);

            return new ClassOverviewResponse
            {
                ClassId = classId,
                ClassName = classEntity.ClassName ?? "Unnamed Class",
                CourseId = courseId,
                CourseName = courseName,
                AttendanceActivity = attendanceActivity,
                Performance = performance,
                Engagement = engagement,
                Operational = operational,
                TeacherEffectiveness = teacherEffectiveness,
                GeneratedAt = now
            };
        }

        public async Task<ClassListResponse> GetAllClassesOverviewAsync(ClassFilterRequest filter)
        {
            var query = _context.ACAD_Classes
                .Include(c => c.ClassStatus)
                .Include(c => c.TeacherAssignment)
                    .ThenInclude(t => t!.Teacher)
                        .ThenInclude(t => t!.Account)
                .Include(c => c.ACAD_Enrollments)
                    .ThenInclude(e => e.Course)
                .Where(c => !c.IsDeleted)
                .AsQueryable();

            // Apply filters
            if (filter.CourseId.HasValue)
            {
                query = query.Where(c => c.ACAD_Enrollments.Any(e => e.CourseID == filter.CourseId.Value));
            }

            if (filter.TeacherId.HasValue)
            {
                query = query.Where(c => c.TeacherAssignment != null && 
                    c.TeacherAssignment.TeacherID == filter.TeacherId.Value);
            }

            if (!string.IsNullOrEmpty(filter.ClassStatus))
            {
                query = query.Where(c => c.ClassStatus.Code == filter.ClassStatus);
            }

            if (filter.StartDateFrom.HasValue)
            {
                query = query.Where(c => c.StartDate >= DateOnly.FromDateTime(filter.StartDateFrom.Value));
            }

            if (filter.StartDateTo.HasValue)
            {
                query = query.Where(c => c.StartDate <= DateOnly.FromDateTime(filter.StartDateTo.Value));
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(c => c.IsActive == filter.IsActive.Value);
            }

            var totalCount = await query.CountAsync();

            var classes = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var summaries = new List<ClassSummaryResponse>();

            foreach (var classEntity in classes)
            {
                var studentIds = classEntity.ACAD_Enrollments
                    .Where(e => !e.IsDeleted)
                    .Select(e => e.StudentID)
                    .ToList();

                // Get attendance rate
                var totalMeetings = await _context.ACAD_ClassMeetings
                    .Where(m => m.ClassID == classEntity.Id && !m.IsDeleted)
                    .CountAsync();

                var totalExpectedAttendances = totalMeetings * studentIds.Count;
                var totalAttendances = await _context.ACAD_Attendances
                    .Where(a => studentIds.Contains(a.StudentID) && 
                               a.Meeting.ClassID == classEntity.Id)
                    .CountAsync();

                var attendanceRate = totalExpectedAttendances > 0
                    ? (decimal)totalAttendances / totalExpectedAttendances * 100
                    : 0;

                // Get average score
                var avgScore = await _context.ACAD_Submissions
                    .Where(s => !s.IsDeleted && 
                               studentIds.Contains(s.StudentID) &&
                               s.Assignment != null &&
                               s.Assignment.ClassMeeting != null &&
                               s.Assignment.ClassMeeting.ClassID == classEntity.Id &&
                               s.Score.HasValue)
                    .AverageAsync(s => (decimal?)s.Score) ?? 0;

                // Get completion rate
                var completedStatus = await _context.CORE_LookUps
                    .Where(l => l.LookUpType.Code == "EnrollmentStatus" && l.Code == "Completed")
                    .Select(l => l.Id)
                    .FirstOrDefaultAsync();

                var completedCount = classEntity.ACAD_Enrollments
                    .Count(e => !e.IsDeleted && e.EnrollmentStatusID == completedStatus);

                var completionRate = studentIds.Count > 0
                    ? (decimal)completedCount / studentIds.Count * 100
                    : 0;

                var course = classEntity.ACAD_Enrollments.FirstOrDefault()?.Course;

                summaries.Add(new ClassSummaryResponse
                {
                    ClassId = classEntity.Id,
                    ClassName = classEntity.ClassName ?? "Unnamed Class",
                    CourseId = course?.Id ?? Guid.Empty,
                    CourseName = course?.CourseName ?? "Unknown",
                    ClassStatus = classEntity.ClassStatus?.Code ?? "Unknown",
                    Capacity = classEntity.Capacity,
                    EnrolledCount = classEntity.EnrolledCount,
                    CapacityUtilization = classEntity.Capacity > 0
                        ? (decimal)classEntity.EnrolledCount / classEntity.Capacity * 100
                        : 0,
                    AttendanceRate = Math.Round(attendanceRate, 2),
                    AverageScore = Math.Round(avgScore, 2),
                    CompletionRate = Math.Round(completionRate, 2),
                    StartDate = classEntity.StartDate,
                    EndDate = classEntity.EndDate,
                    TeacherName = classEntity.TeacherAssignment?.Teacher?.Account?.FullName,
                    GeneratedAt = DateTime.Now
                });
            }

            return new ClassListResponse
            {
                Classes = summaries,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / filter.PageSize)
            };
        }

        public async Task<ClassSummaryResponse?> GetClassSummaryAsync(Guid classId)
        {
            var classEntity = await _context.ACAD_Classes
                .Include(c => c.ClassStatus)
                .Include(c => c.TeacherAssignment)
                    .ThenInclude(t => t!.Teacher)
                        .ThenInclude(t => t!.Account)
                .Include(c => c.ACAD_Enrollments)
                    .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(c => c.Id == classId && !c.IsDeleted);

            if (classEntity == null) return null;

            var studentIds = classEntity.ACAD_Enrollments
                .Where(e => !e.IsDeleted)
                .Select(e => e.StudentID)
                .ToList();

            // Calculate metrics (simplified version)
            var totalMeetings = await _context.ACAD_ClassMeetings
                .Where(m => m.ClassID == classId && !m.IsDeleted)
                .CountAsync();

            var totalExpectedAttendances = totalMeetings * studentIds.Count;
            var totalAttendances = await _context.ACAD_Attendances
                .Where(a => studentIds.Contains(a.StudentID) && 
                           a.Meeting.ClassID == classId)
                .CountAsync();

            var attendanceRate = totalExpectedAttendances > 0
                ? (decimal)totalAttendances / totalExpectedAttendances * 100
                : 0;

            var avgScore = await _context.ACAD_Submissions
                .Where(s => !s.IsDeleted && 
                           studentIds.Contains(s.StudentID) &&
                           s.Assignment != null &&
                           s.Assignment.ClassMeeting != null &&
                           s.Assignment.ClassMeeting.ClassID == classId &&
                           s.Score.HasValue)
                .AverageAsync(s => (decimal?)s.Score) ?? 0;

            var completedStatus = await _context.CORE_LookUps
                .Where(l => l.LookUpType.Code == "EnrollmentStatus" && l.Code == "Completed")
                .Select(l => l.Id)
                .FirstOrDefaultAsync();

            var completedCount = classEntity.ACAD_Enrollments
                .Count(e => !e.IsDeleted && e.EnrollmentStatusID == completedStatus);

            var completionRate = studentIds.Count > 0
                ? (decimal)completedCount / studentIds.Count * 100
                : 0;

            var course = classEntity.ACAD_Enrollments.FirstOrDefault()?.Course;

            return new ClassSummaryResponse
            {
                ClassId = classEntity.Id,
                ClassName = classEntity.ClassName ?? "Unnamed Class",
                CourseId = course?.Id ?? Guid.Empty,
                CourseName = course?.CourseName ?? "Unknown",
                ClassStatus = classEntity.ClassStatus?.Code ?? "Unknown",
                Capacity = classEntity.Capacity,
                EnrolledCount = classEntity.EnrolledCount,
                CapacityUtilization = classEntity.Capacity > 0
                    ? (decimal)classEntity.EnrolledCount / classEntity.Capacity * 100
                    : 0,
                AttendanceRate = Math.Round(attendanceRate, 2),
                AverageScore = Math.Round(avgScore, 2),
                CompletionRate = Math.Round(completionRate, 2),
                StartDate = classEntity.StartDate,
                EndDate = classEntity.EndDate,
                TeacherName = classEntity.TeacherAssignment?.Teacher?.Account?.FullName,
                GeneratedAt = DateTime.Now
            };
        }

        #region Private Helper Methods

        private async Task<AttendanceActivityMetrics> GetAttendanceActivityMetricsAsync(
            Guid classId, 
            List<Guid> studentIds, 
            Domain.Entities.ACAD_Class classEntity)
        {
            var now = DateTime.Now;
            var today = DateOnly.FromDateTime(now);

            // Get all meetings for this class
            var meetings = await _context.ACAD_ClassMeetings
                .Where(m => m.ClassID == classId && !m.IsDeleted)
                .OrderBy(m => m.Date)
                .ToListAsync();

            var totalMeetings = meetings.Count;
            var completedMeetings = meetings.Count(m => m.Date < today);
            var remainingMeetings = meetings.Count(m => m.Date >= today);

            // Calculate attendance
            var totalExpectedAttendances = totalMeetings * studentIds.Count;
            var totalAttendances = await _context.ACAD_Attendances
                .Where(a => studentIds.Contains(a.StudentID) && 
                           a.Meeting.ClassID == classId)
                .CountAsync();

            var attendanceRate = totalExpectedAttendances > 0
                ? (decimal)totalAttendances / totalExpectedAttendances * 100
                : 0;

            var avgAttendancePerMeeting = totalMeetings > 0
                ? (decimal)totalAttendances / totalMeetings
                : 0;

            // Get absence patterns
            var absencePatterns = new List<AbsencePatternData>();
            foreach (var studentId in studentIds)
            {
                var student = await _context.IDN_Students
                    .Include(s => s.Account)
                    .FirstOrDefaultAsync(s => s.Id == studentId);

                var studentMeetings = meetings.Count;
                var studentAttendances = await _context.ACAD_Attendances
                    .Where(a => a.StudentID == studentId && 
                               a.Meeting.ClassID == classId)
                    .CountAsync();

                var absences = studentMeetings - studentAttendances;
                var absenceRate = studentMeetings > 0
                    ? (decimal)absences / studentMeetings * 100
                    : 0;

                var lastAbsence = await _context.ACAD_ClassMeetings
                    .Where(m => m.ClassID == classId && 
                               !m.ACAD_Attendances.Any(a => a.StudentID == studentId))
                    .OrderByDescending(m => m.Date)
                    .Select(m => m.Date)
                    .FirstOrDefaultAsync();

                absencePatterns.Add(new AbsencePatternData
                {
                    StudentId = studentId,
                    StudentName = student?.Account?.FullName ?? "Unknown",
                    TotalAbsences = absences,
                    AbsenceRate = Math.Round(absenceRate, 2),
                    Pattern = absenceRate > 30 ? "Frequent" : absenceRate > 10 ? "Occasional" : "Rare",
                    LastAbsenceDate = lastAbsence != default ? (DateTime?)new DateTime(lastAbsence.Year, lastAbsence.Month, lastAbsence.Day) : null
                });
            }

            // Get check-in trend
            var checkInTrend = new List<CheckInTrendData>();
            foreach (var meeting in meetings.Take(20)) // Last 20 meetings
            {
                var expectedAttendees = studentIds.Count;
                var actualAttendees = await _context.ACAD_Attendances
                    .Where(a => a.MeetingID == meeting.Id && studentIds.Contains(a.StudentID))
                    .CountAsync();

                var meetingAttendanceRate = expectedAttendees > 0
                    ? (decimal)actualAttendees / expectedAttendees * 100
                    : 0;

                checkInTrend.Add(new CheckInTrendData
                {
                    Date = new DateTime(meeting.Date.Year, meeting.Date.Month, meeting.Date.Day),
                    ExpectedAttendees = expectedAttendees,
                    ActualAttendees = actualAttendees,
                    AttendanceRate = Math.Round(meetingAttendanceRate, 2),
                    MeetingStatus = meeting.Date < today ? "Completed" : meeting.Date == today ? "Scheduled" : "Upcoming"
                });
            }

            var perfectAttendanceCount = absencePatterns.Count(a => a.TotalAbsences == 0);
            var highAbsenceCount = absencePatterns.Count(a => a.AbsenceRate > 30);

            var classDensity = classEntity.Capacity > 0
                ? (decimal)classEntity.EnrolledCount / classEntity.Capacity * 100
                : 0;

            return new AttendanceActivityMetrics
            {
                AttendanceRate = Math.Round(attendanceRate, 2),
                TotalMeetings = totalMeetings,
                CompletedMeetings = completedMeetings,
                RemainingMeetings = remainingMeetings,
                AverageAttendancePerMeeting = Math.Round(avgAttendancePerMeeting, 2),
                AbsencePatterns = absencePatterns.OrderByDescending(a => a.AbsenceRate).ToList(),
                CheckInTrend = checkInTrend.OrderBy(c => c.Date).ToList(),
                ClassDensity = Math.Round(classDensity, 2),
                PerfectAttendanceCount = perfectAttendanceCount,
                HighAbsenceCount = highAbsenceCount
            };
        }

        private async Task<ClassPerformanceMetrics> GetPerformanceMetricsAsync(
            Guid classId, 
            List<Guid> studentIds, 
            List<Domain.Entities.ACAD_Enrollment> enrollments)
        {
            // Get all assignments for this class
            var assignments = await _context.ACAD_Assignments
                .Include(a => a.ClassMeeting)
                .Where(a => a.ClassMeeting != null && 
                           a.ClassMeeting.ClassID == classId &&
                           !a.IsDeleted)
                .ToListAsync();

            var assignmentIds = assignments.Select(a => a.Id).ToList();

            // Get all submissions
            var submissions = await _context.ACAD_Submissions
                .Where(s => assignmentIds.Contains(s.AssignmentID!.Value) &&
                           studentIds.Contains(s.StudentID) &&
                           !s.IsDeleted &&
                           s.Score.HasValue)
                .ToListAsync();

            var avgScore = submissions.Count > 0
                ? submissions.Average(s => s.Score!.Value)
                : 0;

            // Completion rate
            var completedStatus = await _context.CORE_LookUps
                .Where(l => l.LookUpType.Code == "EnrollmentStatus" && l.Code == "Completed")
                .Select(l => l.Id)
                .FirstOrDefaultAsync();

            var completedStudents = enrollments.Count(e => e.EnrollmentStatusID == completedStatus);
            var completionRate = studentIds.Count > 0
                ? (decimal)completedStudents / studentIds.Count * 100
                : 0;

            // Pass rate (assuming pass = score >= 5.0)
            var passedStudents = submissions
                .GroupBy(s => s.StudentID)
                .Where(g => g.Average(s => s.Score!.Value) >= 5.0m)
                .Count();

            var passRate = studentIds.Count > 0
                ? (decimal)passedStudents / studentIds.Count * 100
                : 0;

            // Student performances
            var studentPerformances = new List<StudentPerformanceData>();
            foreach (var studentId in studentIds)
            {
                var student = await _context.IDN_Students
                    .Include(s => s.Account)
                    .FirstOrDefaultAsync(s => s.Id == studentId);

                var studentSubmissions = submissions.Where(s => s.StudentID == studentId).ToList();
                var studentAvgScore = studentSubmissions.Count > 0
                    ? (decimal)studentSubmissions.Average(s => s.Score!.Value)
                    : 0m;

                var studentCompletedAssignments = studentSubmissions.Count;
                var studentTotalAssignments = assignments.Count;

                var studentCompletionRate = studentTotalAssignments > 0
                    ? (decimal)studentCompletedAssignments / studentTotalAssignments * 100
                    : 0;

                var studentAttendanceRate = await GetStudentAttendanceRateAsync(classId, studentId);

                var performanceStatus = studentAvgScore >= 8m ? "Excellent" :
                                       studentAvgScore >= 6.5m ? "Good" :
                                       studentAvgScore >= 5m ? "Average" :
                                       "Poor";

                studentPerformances.Add(new StudentPerformanceData
                {
                    StudentId = studentId,
                    StudentName = student?.Account?.FullName ?? "Unknown",
                    AverageScore = Math.Round(studentAvgScore, 2),
                    CompletedAssignments = studentCompletedAssignments,
                    TotalAssignments = studentTotalAssignments,
                    CompletionRate = Math.Round(studentCompletionRate, 2),
                    PerformanceStatus = performanceStatus,
                    AttendanceRate = Math.Round(studentAttendanceRate, 2)
                });
            }

            // Top students
            var topStudents = studentPerformances
                .OrderByDescending(s => s.AverageScore)
                .Take(5)
                .Select((s, index) => new TopStudentData
                {
                    StudentId = s.StudentId,
                    StudentName = s.StudentName,
                    AverageScore = s.AverageScore,
                    Rank = index + 1
                })
                .ToList();

            // At-risk students
            var atRiskStudents = studentPerformances
                .Where(s => s.AverageScore < 5 || s.AttendanceRate < 70 || s.CompletionRate < 50)
                .OrderBy(s => s.AverageScore)
                .Select(s => new AtRiskStudentData
                {
                    StudentId = s.StudentId,
                    StudentName = s.StudentName,
                    AverageScore = s.AverageScore,
                    AttendanceRate = s.AttendanceRate,
                    MissedAssignments = s.TotalAssignments - s.CompletedAssignments,
                    RiskLevel = s.AverageScore < 4 || s.AttendanceRate < 50 ? "High" :
                               s.AverageScore < 5 || s.AttendanceRate < 70 ? "Medium" :
                               "Low"
                })
                .ToList();

            // Dropped students
            var droppedStatus = await _context.CORE_LookUps
                .Where(l => l.LookUpType.Code == "EnrollmentStatus" && 
                           (l.Code == "Dropped" || l.Code == "Cancelled"))
                .Select(l => l.Id)
                .ToListAsync();

            var droppedStudents = enrollments.Count(e => droppedStatus.Contains(e.EnrollmentStatusID));

            // Progress achievement (simplified)
            var progressAchievementRate = studentPerformances.Count > 0
                ? studentPerformances.Average(s => s.CompletionRate)
                : 0;

            return new ClassPerformanceMetrics
            {
                AverageScore = Math.Round((decimal)avgScore, 2),
                CompletionRate = Math.Round(completionRate, 2),
                PassRate = Math.Round(passRate, 2),
                CompletedStudents = completedStudents,
                DroppedStudents = droppedStudents,
                ProgressAchievementRate = Math.Round(progressAchievementRate, 2),
                StudentPerformances = studentPerformances.OrderByDescending(s => s.AverageScore).ToList(),
                PerformanceTrend = new List<PerformanceTrendData>(), // TODO: Implement trend
                TopStudents = topStudents,
                AtRiskStudents = atRiskStudents
            };
        }

        private async Task<ClassEngagementMetrics> GetEngagementMetricsAsync(
            Guid classId, 
            List<Guid> studentIds)
        {
            // Get assignments
            var assignments = await _context.ACAD_Assignments
                .Include(a => a.ClassMeeting)
                .Where(a => a.ClassMeeting != null && 
                           a.ClassMeeting.ClassID == classId &&
                           !a.IsDeleted)
                .ToListAsync();

            var assignmentIds = assignments.Select(a => a.Id).ToList();

            // Get submissions
            var submissions = await _context.ACAD_Submissions
                .Where(s => assignmentIds.Contains(s.AssignmentID!.Value) &&
                           studentIds.Contains(s.StudentID) &&
                           !s.IsDeleted)
                .ToListAsync();

            var submissionRate = assignments.Count > 0 && studentIds.Count > 0
                ? (decimal)submissions.Count / (assignments.Count * studentIds.Count) * 100
                : 0;

            // On-time submissions
            var onTimeSubmissions = submissions.Count(s => 
                s.Assignment != null && 
                s.Assignment.DueAt.HasValue &&
                s.CreatedAt <= s.Assignment.DueAt.Value);

            var onTimeRate = submissions.Count > 0
                ? (decimal)onTimeSubmissions / submissions.Count * 100
                : 0;

            // Get feedback - Feedback is linked to Course, not Class directly
            // We'll get feedbacks for the course this class belongs to
            var courseId = await _context.ACAD_Enrollments
                .Where(e => e.ClassID == classId && !e.IsDeleted)
                .Select(e => e.CourseID)
                .FirstOrDefaultAsync();

            var feedbacks = await _context.COM_Feedbacks
                .Where(f => !f.IsDeleted && 
                           f.CourseID == courseId &&
                           f.Rating.HasValue)
                .ToListAsync();

            var avgFeedbackRating = feedbacks.Count > 0
                ? feedbacks.Average(f => f.Rating!.Value)
                : 0;

            // Participation level (based on attendance + submissions)
            var totalMeetings = await _context.ACAD_ClassMeetings
                .Where(m => m.ClassID == classId && !m.IsDeleted)
                .CountAsync();

            var totalExpectedAttendances = totalMeetings * studentIds.Count;
            var totalAttendances = await _context.ACAD_Attendances
                .Where(a => studentIds.Contains(a.StudentID) && 
                           a.Meeting.ClassID == classId)
                .CountAsync();

            var attendanceRate = totalExpectedAttendances > 0
                ? (decimal)totalAttendances / totalExpectedAttendances * 100
                : 0;

            var participationLevel = (attendanceRate + submissionRate) / 2;

            // High/Low engagement students
            var highEngagement = studentIds.Count(s => 
                submissions.Count(sub => sub.StudentID == s) > assignments.Count * 0.8m &&
                totalAttendances > totalMeetings * 0.8m);

            var lowEngagement = studentIds.Count(s => 
                submissions.Count(sub => sub.StudentID == s) < assignments.Count * 0.3m ||
                totalAttendances < totalMeetings * 0.5m);

            return new ClassEngagementMetrics
            {
                ParticipationLevel = Math.Round(participationLevel, 2),
                InteractionRate = Math.Round(participationLevel, 2), // Simplified
                FeedbackCount = feedbacks.Count,
                AverageFeedbackRating = Math.Round((decimal)avgFeedbackRating, 2),
                AssignmentSubmissions = submissions.Count,
                TotalAssignments = assignments.Count,
                AssignmentSubmissionRate = Math.Round(submissionRate, 2),
                OnTimeSubmissionRate = Math.Round(onTimeRate, 2),
                HighEngagementStudents = highEngagement,
                LowEngagementStudents = lowEngagement
            };
        }

        private ClassOperationalMetrics GetOperationalMetrics(
            Domain.Entities.ACAD_Class classEntity, 
            DateOnly today)
        {
            var startDate = classEntity.StartDate;
            var endDate = classEntity.EndDate;
            var daysElapsed = today.DayNumber - startDate.DayNumber;
            var daysRemaining = endDate.DayNumber - today.DayNumber;
            var totalDays = endDate.DayNumber - startDate.DayNumber;

            var classProgress = totalDays > 0
                ? (decimal)daysElapsed / totalDays * 100
                : 0;

            var capacityUtilization = classEntity.Capacity > 0
                ? (decimal)classEntity.EnrolledCount / classEntity.Capacity * 100
                : 0;

            var classStatus = today < startDate ? "Upcoming" :
                             today > endDate ? "Completed" :
                             classEntity.IsActive ? "Active" :
                             "Inactive";

            return new ClassOperationalMetrics
            {
                ClassStatus = classStatus,
                Capacity = classEntity.Capacity,
                EnrolledCount = classEntity.EnrolledCount,
                CapacityUtilization = Math.Round(capacityUtilization, 2),
                AvailableSpots = classEntity.Capacity - classEntity.EnrolledCount,
                StartDate = startDate,
                EndDate = endDate,
                TotalLessons = 0, // TODO: Calculate from syllabus
                CompletedLessons = 0, // TODO: Calculate from meetings
                RemainingLessons = 0, // TODO: Calculate
                ClassDurationDays = totalDays > 0 ? totalDays : 0,
                DaysElapsed = daysElapsed > 0 ? daysElapsed : 0,
                DaysRemaining = daysRemaining > 0 ? daysRemaining : 0,
                ClassProgress = Math.Round(classProgress, 2)
            };
        }

        private async Task<TeacherEffectivenessMetrics> GetTeacherEffectivenessMetricsAsync(
            Domain.Entities.ACAD_Class classEntity, 
            List<Guid> studentIds)
        {
            if (classEntity.TeacherAssignment == null)
            {
                return new TeacherEffectivenessMetrics();
            }

            var teacher = classEntity.TeacherAssignment.Teacher;
            if (teacher == null) return new TeacherEffectivenessMetrics();

            // Get teacher feedback - Feedback is linked to Course, not Class
            var courseId = await _context.ACAD_Enrollments
                .Where(e => e.ClassID == classEntity.Id && !e.IsDeleted)
                .Select(e => e.CourseID)
                .FirstOrDefaultAsync();

            var feedbacks = await _context.COM_Feedbacks
                .Where(f => !f.IsDeleted && 
                           f.TeacherID == teacher.Id &&
                           f.CourseID == courseId &&
                           f.Rating.HasValue)
                .ToListAsync();

            var teacherRating = feedbacks.Count > 0
                ? feedbacks.Average(f => f.Rating!.Value)
                : 0;

            // Teacher punctuality (simplified - based on meetings started on time)
            var meetings = await _context.ACAD_ClassMeetings
                .Where(m => m.ClassID == classEntity.Id && !m.IsDeleted)
                .ToListAsync();

            var teacherPunctuality = 100m; // TODO: Calculate based on actual meeting times

            // Student progress impact (simplified)
            var submissions = await _context.ACAD_Submissions
                .Include(s => s.Assignment)
                    .ThenInclude(a => a!.ClassMeeting)
                .Where(s => !s.IsDeleted &&
                           studentIds.Contains(s.StudentID) &&
                           s.Assignment != null &&
                           s.Assignment.ClassMeeting != null &&
                           s.Assignment.ClassMeeting.ClassID == classEntity.Id &&
                           s.Score.HasValue)
                .ToListAsync();

            var avgScore = submissions.Count > 0
                ? submissions.Average(s => s.Score!.Value)
                : 0;

            var studentProgressImpact = avgScore; // Simplified

            // Total classes taught by this teacher
            var totalClasses = await _context.ACAD_Classes
                .Where(c => !c.IsDeleted && 
                           c.TeacherAssignmentID == classEntity.TeacherAssignmentID)
                .CountAsync();

            // Average completion rate
            var completedStatus = await _context.CORE_LookUps
                .Where(l => l.LookUpType.Code == "EnrollmentStatus" && l.Code == "Completed")
                .Select(l => l.Id)
                .FirstOrDefaultAsync();

            var avgCompletionRate = 75m; // TODO: Calculate from all classes

            // Teacher attendance
            var teacherAttendanceRate = 100m; // TODO: Calculate

            return new TeacherEffectivenessMetrics
            {
                TeacherId = teacher.Id,
                TeacherName = teacher.Account?.FullName ?? "Unknown",
                TeacherRating = Math.Round((decimal)teacherRating, 2),
                TeacherPunctuality = Math.Round(teacherPunctuality, 2),
                FeedbackCount = feedbacks.Count,
                StudentProgressImpact = Math.Round((decimal)studentProgressImpact, 2),
                TotalClassesTaught = totalClasses,
                AverageClassCompletionRate = Math.Round(avgCompletionRate, 2),
                TeacherAttendanceRate = Math.Round(teacherAttendanceRate, 2)
            };
        }

        private async Task<decimal> GetStudentAttendanceRateAsync(Guid classId, Guid studentId)
        {
            var totalMeetings = await _context.ACAD_ClassMeetings
                .Where(m => m.ClassID == classId && !m.IsDeleted)
                .CountAsync();

            if (totalMeetings == 0) return 0;

            var attendances = await _context.ACAD_Attendances
                .Where(a => a.StudentID == studentId && 
                           a.Meeting.ClassID == classId)
                .CountAsync();

            return (decimal)attendances / totalMeetings * 100;
        }

        #endregion
    }
}

