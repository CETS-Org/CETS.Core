using Domain.Data;
using Domain.Interfaces.Analytics;
using DTOs.Analytics.OverallOverview.Responses;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.Analytics
{
    public class AnalyticsRepository : IAnalyticsRepository
    {
        private readonly AppDbContext _context;

        public AnalyticsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<StudentMetricsResponse> GetStudentMetricsAsync()
        {
            var now = DateTime.Now;
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            var firstDayOfLastMonth = firstDayOfMonth.AddMonths(-1);

            var totalStudents = await _context.IDN_Students
                .Where(s => !s.IsDeleted)
                .CountAsync();

            var newStudentsThisMonth = await _context.IDN_Students
                .Where(s => !s.IsDeleted && s.CreatedAt >= firstDayOfMonth)
                .CountAsync();

            var studentsLastMonth = await _context.IDN_Students
                .Where(s => !s.IsDeleted && s.CreatedAt < firstDayOfMonth)
                .CountAsync();

            // Get active enrollment status
            var activeEnrollmentStatuses = await _context.CORE_LookUps
                .Where(l => l.LookUpType.Code == "EnrollmentStatus" && 
                           (l.Code == "Active" || l.Code == "InProgress"))
                .Select(l => l.Id)
                .ToListAsync();

            var activeStudents = await _context.ACAD_Enrollments
                .Where(e => !e.IsDeleted && activeEnrollmentStatuses.Contains(e.EnrollmentStatusID))
                .Select(e => e.StudentID)
                .Distinct()
                .CountAsync();

            var completedEnrollmentStatuses = await _context.CORE_LookUps
                .Where(l => l.LookUpType.Code == "EnrollmentStatus" && l.Code == "Completed")
                .Select(l => l.Id)
                .ToListAsync();

            var studentsWithCompletedCourses = await _context.ACAD_Enrollments
                .Where(e => !e.IsDeleted && completedEnrollmentStatuses.Contains(e.EnrollmentStatusID))
                .Select(e => e.StudentID)
                .Distinct()
                .CountAsync();

            var totalEnrollments = await _context.ACAD_Enrollments
                .Where(e => !e.IsDeleted)
                .CountAsync();

            var avgEnrollmentsPerStudent = totalStudents > 0 
                ? (decimal)totalEnrollments / totalStudents 
                : 0;

            var activeStudentRate = totalStudents > 0 
                ? (decimal)activeStudents / totalStudents * 100 
                : 0;

            var growthRate = studentsLastMonth > 0 
                ? (decimal)(totalStudents - studentsLastMonth) / studentsLastMonth * 100 
                : 0;

            return new StudentMetricsResponse
            {
                TotalStudents = totalStudents,
                NewStudentsThisMonth = newStudentsThisMonth,
                ActiveStudents = activeStudents,
                ActiveStudentRate = Math.Round(activeStudentRate, 2),
                StudentsWithCompletedCourses = studentsWithCompletedCourses,
                AverageEnrollmentsPerStudent = Math.Round(avgEnrollmentsPerStudent, 2),
                MonthOverMonthGrowthRate = Math.Round(growthRate, 2)
            };
        }

        public async Task<FinancialMetricsResponse> GetFinancialMetricsAsync()
        {
            var now = DateTime.Now;
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            var firstDayOfYear = new DateTime(now.Year, 1, 1);
            var firstDayOfLastMonth = firstDayOfMonth.AddMonths(-1);

            // Get invoice statuses - Paid includes: Paid, 1stPaid, 2ndPaid
            var paidStatuses = await _context.CORE_LookUps
                .Where(l => l.LookUpType.Code == "InvoiceStatus" && 
                           (l.Code == "Paid" || l.Code == "1stPaid" || l.Code == "2ndPaid"))
                .Select(l => l.Id)
                .ToListAsync();

            var pendingStatus = await _context.CORE_LookUps
                .Where(l => l.LookUpType.Code == "InvoiceStatus" && l.Code == "Pending")
                .Select(l => l.Id)
                .FirstOrDefaultAsync();

            var totalRevenue = await _context.FIN_Invoices
                .Where(i => paidStatuses.Contains(i.InvoiceStatusID))
                .SumAsync(i => (decimal?)i.TotalAmount) ?? 0;

            var monthlyRevenue = await _context.FIN_Invoices
                .Where(i => paidStatuses.Contains(i.InvoiceStatusID) && i.CreatedAt >= firstDayOfMonth)
                .SumAsync(i => (decimal?)i.TotalAmount) ?? 0;

            var yearlyRevenue = await _context.FIN_Invoices
                .Where(i => paidStatuses.Contains(i.InvoiceStatusID) && i.CreatedAt >= firstDayOfYear)
                .SumAsync(i => (decimal?)i.TotalAmount) ?? 0;

            var lastMonthRevenue = await _context.FIN_Invoices
                .Where(i => paidStatuses.Contains(i.InvoiceStatusID) && 
                           i.CreatedAt >= firstDayOfLastMonth && 
                           i.CreatedAt < firstDayOfMonth)
                .SumAsync(i => (decimal?)i.TotalAmount) ?? 0;

            var totalStudents = await _context.IDN_Students
                .Where(s => !s.IsDeleted)
                .CountAsync();

            var avgRevenuePerStudent = totalStudents > 0 
                ? totalRevenue / totalStudents 
                : 0;

            var paidInvoicesCount = await _context.FIN_Invoices
                .Where(i => paidStatuses.Contains(i.InvoiceStatusID))
                .CountAsync();

            var pendingInvoicesCount = await _context.FIN_Invoices
                .Where(i => i.InvoiceStatusID == pendingStatus)
                .CountAsync();

            var currentDate = DateOnly.FromDateTime(now);
            var overdueInvoicesCount = await _context.FIN_Invoices
                .Where(i => i.InvoiceStatusID == pendingStatus && 
                           i.DueDate.HasValue && 
                           i.DueDate.Value < currentDate)
                .CountAsync();

            var totalInvoices = await _context.FIN_Invoices.CountAsync();
            var overdueRate = totalInvoices > 0 
                ? (decimal)overdueInvoicesCount / totalInvoices * 100 
                : 0;

            var installmentInvoicesCount = await _context.FIN_Invoices
                .Where(i => i.IsInstallment && paidStatuses.Contains(i.InvoiceStatusID))
                .CountAsync();

            var installmentRevenue = await _context.FIN_Invoices
                .Where(i => i.IsInstallment && paidStatuses.Contains(i.InvoiceStatusID))
                .SumAsync(i => (decimal?)i.TotalAmount) ?? 0;

            var revenueGrowth = lastMonthRevenue > 0 
                ? (monthlyRevenue - lastMonthRevenue) / lastMonthRevenue * 100 
                : 0;

            return new FinancialMetricsResponse
            {
                TotalRevenue = totalRevenue,
                MonthlyRevenue = monthlyRevenue,
                YearlyRevenue = yearlyRevenue,
                AverageRevenuePerStudent = Math.Round(avgRevenuePerStudent, 2),
                PaidInvoicesCount = paidInvoicesCount,
                PendingInvoicesCount = pendingInvoicesCount,
                OverdueInvoicesCount = overdueInvoicesCount,
                OverdueRate = Math.Round(overdueRate, 2),
                InstallmentInvoicesCount = installmentInvoicesCount,
                InstallmentRevenue = installmentRevenue,
                MonthOverMonthRevenueGrowth = Math.Round(revenueGrowth, 2)
            };
        }

        public async Task<CourseClassMetricsResponse> GetCourseClassMetricsAsync()
        {
            var now = DateTime.Now;
            var today = DateOnly.FromDateTime(now);

            var totalCourses = await _context.ACAD_Courses
                .Where(c => !c.IsDeleted)
                .CountAsync();

            var totalActiveCourses = await _context.ACAD_Courses
                .Where(c => !c.IsDeleted && c.IsActive)
                .CountAsync();

            var totalClasses = await _context.ACAD_Classes
                .Where(c => !c.IsDeleted)
                .CountAsync();

            var ongoingClasses = await _context.ACAD_Classes
                .Where(c => !c.IsDeleted && 
                           c.StartDate <= today && 
                           c.EndDate >= today && 
                           c.IsActive)
                .CountAsync();

            var upcomingClasses = await _context.ACAD_Classes
                .Where(c => !c.IsDeleted && 
                           c.StartDate > today && 
                           c.IsActive)
                .CountAsync();

            var completedClasses = await _context.ACAD_Classes
                .Where(c => !c.IsDeleted && c.EndDate < today)
                .CountAsync();

            var avgFillRate = await _context.ACAD_Classes
                .Where(c => !c.IsDeleted && c.Capacity > 0)
                .AverageAsync(c => (decimal?)((double)c.EnrolledCount / c.Capacity * 100)) ?? 0;

            var totalEnrollments = await _context.ACAD_Enrollments
                .Where(e => !e.IsDeleted)
                .CountAsync();

            // Get enrollment statuses
            var activeStatus = await _context.CORE_LookUps
                .Where(l => l.LookUpType.Code == "EnrollmentStatus" && 
                           (l.Code == "Enrolled" || l.Code == "Pending" || l.Code == "Transferred"))
                .Select(l => l.Id)
                .ToListAsync();

            var completedStatus = await _context.CORE_LookUps
                .Where(l => l.LookUpType.Code == "EnrollmentStatus" && l.Code == "Enrolled")
                .Select(l => l.Id)
                .ToListAsync();

            var droppedStatus = await _context.CORE_LookUps
                .Where(l => l.LookUpType.Code == "EnrollmentStatus" && 
                           (l.Code == "Dropped" || l.Code == "Refunded"))
                .Select(l => l.Id)
                .ToListAsync();

            var activeEnrollments = await _context.ACAD_Enrollments
                .Where(e => !e.IsDeleted && activeStatus.Contains(e.EnrollmentStatusID))
                .CountAsync();

            var completedEnrollments = await _context.ACAD_Enrollments
                .Where(e => !e.IsDeleted && completedStatus.Contains(e.EnrollmentStatusID))
                .CountAsync();

            var droppedEnrollments = await _context.ACAD_Enrollments
                .Where(e => !e.IsDeleted && droppedStatus.Contains(e.EnrollmentStatusID))
                .CountAsync();

            var completionRate = totalEnrollments > 0 
                ? (decimal)completedEnrollments / totalEnrollments * 100 
                : 0;

            var dropoutRate = totalEnrollments > 0 
                ? (decimal)droppedEnrollments / totalEnrollments * 100 
                : 0;

            return new CourseClassMetricsResponse
            {
                TotalActiveCourses = totalActiveCourses,
                TotalCourses = totalCourses,
                TotalClasses = totalClasses,
                OngoingClasses = ongoingClasses,
                UpcomingClasses = upcomingClasses,
                CompletedClasses = completedClasses,
                AverageClassFillRate = Math.Round(avgFillRate, 2),
                TotalEnrollments = totalEnrollments,
                ActiveEnrollments = activeEnrollments,
                CompletedEnrollments = completedEnrollments,
                DroppedEnrollments = droppedEnrollments,
                EnrollmentCompletionRate = Math.Round(completionRate, 2),
                EnrollmentDropoutRate = Math.Round(dropoutRate, 2)
            };
        }

        public async Task<TeacherMetricsResponse> GetTeacherMetricsAsync()
        {
            var now = DateTime.Now;

            var totalTeachers = await _context.IDN_Teachers
                .Where(t => !t.IsDeleted)
                .CountAsync();

            var activeTeachers = await _context.ACAD_CourseTeacherAssignments
                .Select(a => a.TeacherID)
                .Distinct()
                .CountAsync();

            var teachersWithValidContracts = await _context.HR_Contracts
                .Where(c => !c.IsDeleted && c.ExpiredAt.HasValue && c.ExpiredAt.Value > now)
                .Select(c => c.TeacherID)
                .Distinct()
                .CountAsync();

            var thirtyDaysFromNow = now.AddDays(30);
            var contractsExpiringSoon = await _context.HR_Contracts
                .Where(c => !c.IsDeleted && 
                           c.ExpiredAt.HasValue && 
                           c.ExpiredAt.Value > now && 
                           c.ExpiredAt.Value <= thirtyDaysFromNow)
                .CountAsync();

            var avgYearsExperience = await _context.IDN_Teachers
                .Where(t => !t.IsDeleted && t.YearsExperience.HasValue)
                .AverageAsync(t => (decimal?)t.YearsExperience) ?? 0;

            var avgTeacherRating = await _context.COM_Feedbacks
                .Where(f => !f.IsDeleted && f.TeacherID.HasValue && f.Rating.HasValue)
                .AverageAsync(f => (decimal?)f.Rating) ?? 0;

            var totalClassesTeaching = await _context.ACAD_Classes
                .Where(c => !c.IsDeleted && c.TeacherAssignmentID.HasValue)
                .CountAsync();

            var avgClassesPerTeacher = totalTeachers > 0 
                ? (decimal)totalClassesTeaching / totalTeachers 
                : 0;

            // Estimate teaching hours (assuming 2 hours per meeting)
            var totalMeetings = await _context.ACAD_ClassMeetings
                .Where(m => !m.IsDeleted)
                .CountAsync();

            var totalTeachingHours = totalMeetings * 2; // 2 hours per meeting

            return new TeacherMetricsResponse
            {
                TotalTeachers = totalTeachers,
                ActiveTeachers = activeTeachers,
                TeachersWithValidContracts = teachersWithValidContracts,
                ContractsExpiringSoon = contractsExpiringSoon,
                AverageYearsExperience = Math.Round(avgYearsExperience, 1),
                AverageTeacherRating = Math.Round(avgTeacherRating, 2),
                TotalClassesTeaching = totalClassesTeaching,
                AverageClassesPerTeacher = Math.Round(avgClassesPerTeacher, 2),
                TotalTeachingHours = totalTeachingHours
            };
        }

        public async Task<EventMetricsResponse> GetEventMetricsAsync()
        {
            var now = DateTime.Now;

            var totalEvents = await _context.EVT_Events
                .Where(e => !e.IsDeleted)
                .CountAsync();

            var upcomingEvents = await _context.EVT_Events
                .Where(e => !e.IsDeleted && e.StartDate > now)
                .CountAsync();

            var ongoingEvents = await _context.EVT_Events
                .Where(e => !e.IsDeleted && e.StartDate <= now && e.EndDate >= now)
                .CountAsync();

            var completedEvents = await _context.EVT_Events
                .Where(e => !e.IsDeleted && e.EndDate < now)
                .CountAsync();

            var totalRegistrations = await _context.EVT_EventRegistrations
                .Where(r => !r.IsDeleted)
                .CountAsync();

            var checkedInRegistrations = await _context.EVT_EventRegistrations
                .Where(r => !r.IsDeleted && r.CheckInAt.HasValue)
                .CountAsync();

            var attendanceRate = totalRegistrations > 0 
                ? (decimal)checkedInRegistrations / totalRegistrations * 100 
                : 0;

            var avgRegistrationsPerEvent = totalEvents > 0 
                ? (decimal)totalRegistrations / totalEvents 
                : 0;

            var avgEventRating = await _context.EVT_EventFeedbacks
                .Where(f => f.Rating.HasValue)
                .AverageAsync(f => (decimal?)f.Rating) ?? 0;

            return new EventMetricsResponse
            {
                TotalEvents = totalEvents,
                UpcomingEvents = upcomingEvents,
                OngoingEvents = ongoingEvents,
                CompletedEvents = completedEvents,
                TotalRegistrations = totalRegistrations,
                CheckedInRegistrations = checkedInRegistrations,
                EventAttendanceRate = Math.Round(attendanceRate, 2),
                AverageRegistrationsPerEvent = Math.Round(avgRegistrationsPerEvent, 2),
                AverageEventRating = Math.Round(avgEventRating, 2)
            };
        }

        public async Task<FeedbackMetricsResponse> GetFeedbackMetricsAsync()
        {
            var totalFeedbacks = await _context.COM_Feedbacks
                .Where(f => !f.IsDeleted)
                .CountAsync();

            var overallAvgRating = await _context.COM_Feedbacks
                .Where(f => !f.IsDeleted && f.Rating.HasValue)
                .AverageAsync(f => (decimal?)f.Rating) ?? 0;

            var courseFeedbackCount = await _context.COM_Feedbacks
                .Where(f => !f.IsDeleted && f.CourseID.HasValue)
                .CountAsync();

            var courseAvgRating = await _context.COM_Feedbacks
                .Where(f => !f.IsDeleted && f.CourseID.HasValue && f.Rating.HasValue)
                .AverageAsync(f => (decimal?)f.Rating) ?? 0;

            var teacherFeedbackCount = await _context.COM_Feedbacks
                .Where(f => !f.IsDeleted && f.TeacherID.HasValue)
                .CountAsync();

            var teacherAvgRating = await _context.COM_Feedbacks
                .Where(f => !f.IsDeleted && f.TeacherID.HasValue && f.Rating.HasValue)
                .AverageAsync(f => (decimal?)f.Rating) ?? 0;

            var fiveStarCount = await _context.COM_Feedbacks
                .Where(f => !f.IsDeleted && f.Rating == 5)
                .CountAsync();

            var fourStarCount = await _context.COM_Feedbacks
                .Where(f => !f.IsDeleted && f.Rating == 4)
                .CountAsync();

            var threeStarCount = await _context.COM_Feedbacks
                .Where(f => !f.IsDeleted && f.Rating == 3)
                .CountAsync();

            var twoStarCount = await _context.COM_Feedbacks
                .Where(f => !f.IsDeleted && f.Rating == 2)
                .CountAsync();

            var oneStarCount = await _context.COM_Feedbacks
                .Where(f => !f.IsDeleted && f.Rating == 1)
                .CountAsync();

            // Net Promoter Score: (Promoters % - Detractors %)
            // Promoters: 4-5 stars, Detractors: 1-2 stars
            var promoters = fiveStarCount + fourStarCount;
            var detractors = oneStarCount + twoStarCount;
            var feedbacksWithRating = await _context.COM_Feedbacks
                .Where(f => !f.IsDeleted && f.Rating.HasValue)
                .CountAsync();

            var nps = feedbacksWithRating > 0 
                ? ((decimal)promoters / feedbacksWithRating * 100) - ((decimal)detractors / feedbacksWithRating * 100) 
                : 0;

            // Customer Satisfaction Score: percentage of 4-5 star ratings
            var csat = feedbacksWithRating > 0 
                ? (decimal)promoters / feedbacksWithRating * 100 
                : 0;

            return new FeedbackMetricsResponse
            {
                TotalFeedbacks = totalFeedbacks,
                OverallAverageRating = Math.Round(overallAvgRating, 2),
                CourseAverageRating = Math.Round(courseAvgRating, 2),
                CourseFeedbackCount = courseFeedbackCount,
                TeacherAverageRating = Math.Round(teacherAvgRating, 2),
                TeacherFeedbackCount = teacherFeedbackCount,
                FiveStarCount = fiveStarCount,
                FourStarCount = fourStarCount,
                ThreeStarCount = threeStarCount,
                TwoStarCount = twoStarCount,
                OneStarCount = oneStarCount,
                NetPromoterScore = Math.Round(nps, 2),
                CustomerSatisfactionScore = Math.Round(csat, 2)
            };
        }

        // ===== NEW CATEGORY-BASED METHODS =====

        public async Task<CenterPerformanceResponse> GetCenterPerformanceAsync()
        {
            var now = DateTime.Now;
            var today = DateOnly.FromDateTime(now);
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);

            // Room & Facility Utilization
            var totalRooms = await _context.FAC_Rooms
                .CountAsync();

            var activeRooms = await _context.FAC_Rooms
                .Where(r => r.IsActive)
                .CountAsync();

            // Calculate room occupancy (simplified - based on class meetings)
            var totalRoomHours = activeRooms * 10 * 30; // 10 hours/day * 30 days
            var usedRoomHours = await _context.ACAD_ClassMeetings
                .Where(m => !m.IsDeleted && m.Date.Month == now.Month && m.Date.Year == now.Year)
                .CountAsync() * 2; // Assume 2 hours per meeting

            var roomOccupancyRate = totalRoomHours > 0 
                ? (decimal)usedRoomHours / totalRoomHours * 100 
                : 0;

            // Class Operations
            var classesOpenedThisMonth = await _context.ACAD_Classes
                .Where(c => !c.IsDeleted && c.CreatedAt >= firstDayOfMonth)
                .CountAsync();

            var classesClosedThisMonth = await _context.ACAD_Classes
                .Where(c => !c.IsDeleted && c.EndDate < today && c.EndDate >= DateOnly.FromDateTime(firstDayOfMonth))
                .CountAsync();

            var activeClassesCount = await _context.ACAD_Classes
                .Where(c => !c.IsDeleted && 
                           c.StartDate <= today && 
                           c.EndDate >= today && 
                           c.IsActive)
                .CountAsync();

            // Capacity Metrics
            var capacityData = await _context.ACAD_Classes
                .Where(c => !c.IsDeleted && c.IsActive)
                .Select(c => new { c.Capacity, c.EnrolledCount })
                .ToListAsync();

            var totalStudentCapacity = capacityData.Sum(c => c.Capacity);
            var currentEnrollmentCount = capacityData.Sum(c => c.EnrolledCount);
            var availableCapacity = totalStudentCapacity - currentEnrollmentCount;

            var capacityUtilizationRate = totalStudentCapacity > 0 
                ? (decimal)currentEnrollmentCount / totalStudentCapacity * 100 
                : 0;

            var avgClassFillRate = capacityData.Count > 0 && capacityData.Any(c => c.Capacity > 0)
                ? capacityData.Where(c => c.Capacity > 0)
                                .Average(c => (decimal)c.EnrolledCount / c.Capacity * 100)
                : 0;

            // Overall Utilization (composite of room, capacity, and fill rate)
            var overallUtilizationRate = (roomOccupancyRate + capacityUtilizationRate + avgClassFillRate) / 3;

            // Operational Efficiency Score (0-100)
            var operationalEfficiencyScore = overallUtilizationRate;

            return new CenterPerformanceResponse
            {
                TotalRooms = totalRooms,
                ActiveRooms = activeRooms,
                RoomOccupancyRate = Math.Round(roomOccupancyRate, 2),
                AverageRoomUtilization = Math.Round(roomOccupancyRate, 2),
                ClassesOpenedThisMonth = classesOpenedThisMonth,
                ClassesClosedThisMonth = classesClosedThisMonth,
                ActiveClassesCount = activeClassesCount,
                OverallUtilizationRate = Math.Round(overallUtilizationRate, 2),
                AverageClassFillRate = Math.Round(avgClassFillRate, 2),
                OperationalEfficiencyScore = Math.Round(operationalEfficiencyScore, 2),
                TotalStudentCapacity = totalStudentCapacity,
                CurrentEnrollmentCount = currentEnrollmentCount,
                AvailableCapacity = availableCapacity,
                CapacityUtilizationRate = Math.Round(capacityUtilizationRate, 2)
            };
        }

        public async Task<GrowthRetentionResponse> GetGrowthRetentionAsync()
        {
            var now = DateTime.Now;
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            var firstDayOfLastMonth = firstDayOfMonth.AddMonths(-1);
            var threeMonthsAgo = now.AddMonths(-3);
            var sixMonthsAgo = now.AddMonths(-6);

            // Enrollment Trends
            var totalActiveStudents = await _context.IDN_Students
                .Where(s => !s.IsDeleted)
                .CountAsync();

            var newStudentsThisMonth = await _context.IDN_Students
                .Where(s => !s.IsDeleted && s.CreatedAt >= firstDayOfMonth)
                .CountAsync();

            var newStudentsLastMonth = await _context.IDN_Students
                .Where(s => !s.IsDeleted && 
                           s.CreatedAt >= firstDayOfLastMonth && 
                           s.CreatedAt < firstDayOfMonth)
                .CountAsync();

            var monthOverMonthGrowthRate = newStudentsLastMonth > 0
                ? (decimal)(newStudentsThisMonth - newStudentsLastMonth) / newStudentsLastMonth * 100
                : 0;

            var totalEnrollmentsThisMonth = await _context.ACAD_Enrollments
                .Where(e => !e.IsDeleted && e.CreatedAt >= firstDayOfMonth)
                .CountAsync();

            var enrollmentTrend = monthOverMonthGrowthRate > 5 ? "Increasing" 
                                : monthOverMonthGrowthRate < -5 ? "Decreasing" 
                                : "Stable";

            // Retention Metrics
            var completedStatus = await _context.CORE_LookUps
                .Where(l => l.LookUpType.Code == "EnrollmentStatus" && l.Code == "Completed")
                .Select(l => l.Id)
                .ToListAsync();

            var studentsCompletedLast3Months = await _context.ACAD_Enrollments
                .Where(e => !e.IsDeleted && 
                           completedStatus.Contains(e.EnrollmentStatusID) &&
                           e.UpdatedAt >= threeMonthsAgo)
                .Select(e => e.StudentID)
                .Distinct()
                .ToListAsync();

            var studentsRetained = await _context.ACAD_Enrollments
                .Where(e => !e.IsDeleted && 
                           studentsCompletedLast3Months.Contains(e.StudentID) &&
                           e.CreatedAt >= threeMonthsAgo)
                .Select(e => e.StudentID)
                .Distinct()
                .CountAsync();

            var retentionRate = studentsCompletedLast3Months.Count > 0
                ? (decimal)studentsRetained / studentsCompletedLast3Months.Count * 100
                : 0;

            // Churn Metrics
            var droppedStatus = await _context.CORE_LookUps
                .Where(l => l.LookUpType.Code == "EnrollmentStatus" && 
                           (l.Code == "Dropped" || l.Code == "Refunded"))
                .Select(l => l.Id)
                .ToListAsync();

            var studentsChurnedThisMonth = await _context.ACAD_Enrollments
                .Where(e => !e.IsDeleted && 
                           droppedStatus.Contains(e.EnrollmentStatusID) &&
                           e.UpdatedAt >= firstDayOfMonth)
                .Select(e => e.StudentID)
                .Distinct()
                .CountAsync();

            var studentsChurnedLastMonth = await _context.ACAD_Enrollments
                .Where(e => !e.IsDeleted && 
                           droppedStatus.Contains(e.EnrollmentStatusID) &&
                           e.UpdatedAt >= firstDayOfLastMonth &&
                           e.UpdatedAt < firstDayOfMonth)
                .Select(e => e.StudentID)
                .Distinct()
                .CountAsync();

            var totalEnrollments = await _context.ACAD_Enrollments
                .Where(e => !e.IsDeleted)
                .CountAsync();

            var churnRate = totalEnrollments > 0
                ? (decimal)studentsChurnedThisMonth / totalEnrollments * 100
                : 0;

            var churnTrend = studentsChurnedThisMonth < studentsChurnedLastMonth ? "Improving"
                           : studentsChurnedThisMonth > studentsChurnedLastMonth ? "Worsening"
                           : "Stable";

            // Reactivation
            var inactiveStudents = await _context.IDN_Students
                .Where(s => !s.IsDeleted)
                .Select(s => s.Id)
                .Except(_context.ACAD_Enrollments
                    .Where(e => !e.IsDeleted && e.CreatedAt >= sixMonthsAgo)
                    .Select(e => e.StudentID))
                .ToListAsync();

            var reactivatedStudentsThisMonth = await _context.ACAD_Enrollments
                .Where(e => !e.IsDeleted && 
                           e.CreatedAt >= firstDayOfMonth &&
                           inactiveStudents.Contains(e.StudentID))
                .Select(e => e.StudentID)
                .Distinct()
                .CountAsync();

            var potentialReactivationPool = inactiveStudents.Count;

            var reactivationRate = potentialReactivationPool > 0
                ? (decimal)reactivatedStudentsThisMonth / potentialReactivationPool * 100
                : 0;

            // Lifetime Value Indicators
            var avgCoursesPerStudent = totalActiveStudents > 0
                ? (decimal)totalEnrollments / totalActiveStudents
                : 0;

            // Calculate average student lifetime months
            var studentLifetimes = await _context.IDN_Students
                .Where(s => !s.IsDeleted)
                .Select(s => s.CreatedAt)
                .ToListAsync();
            
            var avgStudentLifetimeMonths = studentLifetimes.Count > 0
                ? (decimal)studentLifetimes.Average(createdAt => (now - createdAt).Days / 30.0)
                : 0;

            var totalRevenue = await _context.FIN_Invoices
                .SumAsync(i => (decimal?)i.TotalAmount) ?? 0;

            var avgStudentLifetimeValue = totalActiveStudents > 0
                ? totalRevenue / totalActiveStudents
                : 0;

            return new GrowthRetentionResponse
            {
                TotalActiveStudents = totalActiveStudents,
                NewStudentsThisMonth = newStudentsThisMonth,
                NewStudentsLastMonth = newStudentsLastMonth,
                MonthOverMonthGrowthRate = Math.Round(monthOverMonthGrowthRate, 2),
                TotalEnrollmentsThisMonth = totalEnrollmentsThisMonth,
                EnrollmentTrend = enrollmentTrend,
                RetentionRate = Math.Round(retentionRate, 2),
                StudentsRetained = studentsRetained,
                StudentsCompletedLast3Months = studentsCompletedLast3Months.Count,
                ChurnRate = Math.Round(churnRate, 2),
                StudentsChurnedThisMonth = studentsChurnedThisMonth,
                StudentsChurnedLastMonth = studentsChurnedLastMonth,
                ChurnTrend = churnTrend,
                ReactivationRate = Math.Round(reactivationRate, 2),
                ReactivatedStudentsThisMonth = reactivatedStudentsThisMonth,
                PotentialReactivationPool = potentialReactivationPool,
                AverageCoursesPerStudent = Math.Round(avgCoursesPerStudent, 2),
                AverageStudentLifetimeMonths = Math.Round(avgStudentLifetimeMonths, 1),
                AverageStudentLifetimeValue = Math.Round(avgStudentLifetimeValue, 2)
            };
        }

        public async Task<RevenueFinanceResponse> GetRevenueFinanceAsync()
        {
            var now = DateTime.Now;
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            var firstDayOfLastMonth = firstDayOfMonth.AddMonths(-1);
            var firstDayOfYear = new DateTime(now.Year, 1, 1);
            var lastYearSameMonth = firstDayOfMonth.AddYears(-1);

            // Get invoice statuses - Paid includes: Paid, 1stPaid, 2ndPaid
            var paidStatuses = await _context.CORE_LookUps
                .Where(l => l.LookUpType.Code == "InvoiceStatus" && 
                           (l.Code == "Paid" || l.Code == "1stPaid" || l.Code == "2ndPaid"))
                .Select(l => l.Id)
                .ToListAsync();

            var pendingStatus = await _context.CORE_LookUps
                .Where(l => l.LookUpType.Code == "InvoiceStatus" && l.Code == "Pending")
                .Select(l => l.Id)
                .FirstOrDefaultAsync();

            // Revenue Trends
            var totalRevenue = await _context.FIN_Invoices
                .Where(i => paidStatuses.Contains(i.InvoiceStatusID))
                .SumAsync(i => (decimal?)i.TotalAmount) ?? 0;

            var revenueThisMonth = await _context.FIN_Invoices
                .Where(i => paidStatuses.Contains(i.InvoiceStatusID) && i.CreateDate >= DateOnly.FromDateTime(firstDayOfMonth))
                .SumAsync(i => (decimal?)i.TotalAmount) ?? 0;

            var revenueLastMonth = await _context.FIN_Invoices
                .Where(i => paidStatuses.Contains(i.InvoiceStatusID) && 
                           i.CreateDate >= DateOnly.FromDateTime(firstDayOfLastMonth) &&
                           i.CreateDate < DateOnly.FromDateTime(firstDayOfMonth))
                .SumAsync(i => (decimal?)i.TotalAmount) ?? 0;

            var revenueThisYear = await _context.FIN_Invoices
                .Where(i => paidStatuses.Contains(i.InvoiceStatusID) && i.CreateDate >= DateOnly.FromDateTime(firstDayOfYear))
                .SumAsync(i => (decimal?)i.TotalAmount) ?? 0;

            var monthOverMonthGrowth = revenueLastMonth > 0
                ? (revenueThisMonth - revenueLastMonth) / revenueLastMonth * 100
                : 0;

            var revenueTrend = monthOverMonthGrowth > 5 ? "Growing"
                             : monthOverMonthGrowth < -5 ? "Declining"
                             : "Stable";

            // Tuition Collection
            var tuitionCollected = totalRevenue; // Simplified - all revenue is tuition
            var tuitionCollectedThisMonth = revenueThisMonth;

            var totalStudents = await _context.IDN_Students.Where(s => !s.IsDeleted).CountAsync();
            var avgTuitionPerStudent = totalStudents > 0 ? tuitionCollected / totalStudents : 0;

            var totalBilled = await _context.FIN_Invoices
                .SumAsync(i => (decimal?)i.TotalAmount) ?? 0;

            var collectionEfficiencyRate = totalBilled > 0
                ? tuitionCollected / totalBilled * 100
                : 0;

            // Pending Payments
            var pendingPaymentAmount = await _context.FIN_Invoices
                .Where(i => i.InvoiceStatusID == pendingStatus)
                .SumAsync(i => (decimal?)i.TotalAmount) ?? 0;

            var pendingInvoicesCount = await _context.FIN_Invoices
                .Where(i => i.InvoiceStatusID == pendingStatus)
                .CountAsync();

            var currentDate = DateOnly.FromDateTime(now);
            var overduePaymentAmount = await _context.FIN_Invoices
                .Where(i => i.InvoiceStatusID == pendingStatus &&
                           i.DueDate.HasValue &&
                           i.DueDate.Value < currentDate)
                .SumAsync(i => (decimal?)i.TotalAmount) ?? 0;

            var overdueInvoicesCount = await _context.FIN_Invoices
                .Where(i => i.InvoiceStatusID == pendingStatus &&
                           i.DueDate.HasValue &&
                           i.DueDate.Value < currentDate)
                .CountAsync();

            var totalInvoices = await _context.FIN_Invoices.CountAsync();
            var overdueRate = totalInvoices > 0
                ? (decimal)overdueInvoicesCount / totalInvoices * 100
                : 0;

            // Refunds
            var totalRefundVolume = await _context.FIN_PaymentRefunds
                .SumAsync(r => (decimal?)r.Amount) ?? 0;

            var refundsThisMonth = await _context.FIN_PaymentRefunds
                .Where(r => r.CreatedAt >= firstDayOfMonth)
                .SumAsync(r => (decimal?)r.Amount) ?? 0;

            var refundTransactionsCount = await _context.FIN_PaymentRefunds
                .CountAsync();

            var refundRate = totalRevenue > 0
                ? totalRefundVolume / totalRevenue * 100
                : 0;

            // Revenue Forecast (simplified - based on historical average)
            var avgMonthlyRevenue = revenueThisYear > 0
                ? revenueThisYear / now.Month
                : 0;

            var forecastedRevenueNextMonth = avgMonthlyRevenue * 1.05m; // 5% growth assumption
            var forecastedRevenueNextQuarter = avgMonthlyRevenue * 3 * 1.05m;

            var activeEnrollmentStatuses = await _context.CORE_LookUps
                .Where(l => l.LookUpType.Code == "EnrollmentStatus" && 
                           (l.Code == "Enrolled" || l.Code == "Pending" || l.Code == "Transferred"))
                .Select(l => l.Id)
                .ToListAsync();

            var pipelineRevenue = await _context.ACAD_Enrollments
                .Where(e => !e.IsDeleted && activeEnrollmentStatuses.Contains(e.EnrollmentStatusID))
                .CountAsync() * 1000; // Simplified - assume 1000 per enrollment

            var forecastConfidence = monthOverMonthGrowth > 10 || monthOverMonthGrowth < -10 ? "Low"
                                   : Math.Abs(monthOverMonthGrowth) < 3 ? "High"
                                   : "Medium";

            // Financial Health
            var avgRevenuePerStudent = totalStudents > 0
                ? totalRevenue / totalStudents
                : 0;

            var revenueLastYearSameMonth = await _context.FIN_Invoices
                .Where(i => paidStatuses.Contains(i.InvoiceStatusID) &&
                           i.CreateDate >= DateOnly.FromDateTime(lastYearSameMonth) &&
                           i.CreateDate < DateOnly.FromDateTime(lastYearSameMonth.AddMonths(1)))
                .SumAsync(i => (decimal?)i.TotalAmount) ?? 0;

            var yearOverYearGrowthRate = revenueLastYearSameMonth > 0
                ? (revenueThisMonth - revenueLastYearSameMonth) / revenueLastYearSameMonth * 100
                : 0;

            // Payment method distribution (simplified)
            var paymentMethodDistribution = new Dictionary<string, int>
            {
                { "Bank Transfer", 0 },
                { "Credit Card", 0 },
                { "Cash", 0 },
                { "E-Wallet", 0 }
            };

            return new RevenueFinanceResponse
            {
                TotalRevenue = totalRevenue,
                RevenueThisMonth = revenueThisMonth,
                RevenueLastMonth = revenueLastMonth,
                RevenueThisYear = revenueThisYear,
                MonthOverMonthGrowth = Math.Round(monthOverMonthGrowth, 2),
                RevenueTrend = revenueTrend,
                TuitionCollected = tuitionCollected,
                TuitionCollectedThisMonth = tuitionCollectedThisMonth,
                AverageTuitionPerStudent = Math.Round(avgTuitionPerStudent, 2),
                CollectionEfficiencyRate = Math.Round(collectionEfficiencyRate, 2),
                PendingPaymentAmount = pendingPaymentAmount,
                PendingInvoicesCount = pendingInvoicesCount,
                OverduePaymentAmount = overduePaymentAmount,
                OverdueInvoicesCount = overdueInvoicesCount,
                OverdueRate = Math.Round(overdueRate, 2),
                TotalRefundVolume = totalRefundVolume,
                RefundsThisMonth = refundsThisMonth,
                RefundTransactionsCount = refundTransactionsCount,
                RefundRate = Math.Round(refundRate, 2),
                ForecastedRevenueNextMonth = Math.Round(forecastedRevenueNextMonth, 2),
                ForecastedRevenueNextQuarter = Math.Round(forecastedRevenueNextQuarter, 2),
                PipelineRevenue = pipelineRevenue,
                ForecastConfidence = forecastConfidence,
                AverageRevenuePerStudent = Math.Round(avgRevenuePerStudent, 2),
                YearOverYearGrowthRate = Math.Round(yearOverYearGrowthRate, 2),
                PaymentMethodDistribution = paymentMethodDistribution
            };
        }

        public async Task<EngagementSatisfactionResponse> GetEngagementSatisfactionAsync()
        {
            var now = DateTime.Now;
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            var firstDayOfLastMonth = firstDayOfMonth.AddMonths(-1);

            // Overall Satisfaction
            var totalFeedbacksReceived = await _context.COM_Feedbacks
                .Where(f => !f.IsDeleted)
                .CountAsync();

            var averageRating = await _context.COM_Feedbacks
                .Where(f => !f.IsDeleted && f.Rating.HasValue)
                .AverageAsync(f => (decimal?)f.Rating) ?? 0;

            var totalStudents = await _context.IDN_Students.Where(s => !s.IsDeleted).CountAsync();
            var feedbackResponseRate = totalStudents > 0
                ? (decimal)totalFeedbacksReceived / totalStudents * 100
                : 0;

            // Student Satisfaction
            var courseSatisfactionRating = await _context.COM_Feedbacks
                .Where(f => !f.IsDeleted && f.CourseID.HasValue && f.Rating.HasValue)
                .AverageAsync(f => (decimal?)f.Rating) ?? 0;

            var teacherSatisfactionRating = await _context.COM_Feedbacks
                .Where(f => !f.IsDeleted && f.TeacherID.HasValue && f.Rating.HasValue)
                .AverageAsync(f => (decimal?)f.Rating) ?? 0;

            var facilitySatisfactionRating = averageRating; // Simplified

            var fiveStarCount = await _context.COM_Feedbacks.Where(f => !f.IsDeleted && f.Rating == 5).CountAsync();
            var fourStarCount = await _context.COM_Feedbacks.Where(f => !f.IsDeleted && f.Rating == 4).CountAsync();

            var promotersCount = fiveStarCount + fourStarCount;
            var studentSatisfactionScore = totalFeedbacksReceived > 0
                ? (decimal)promotersCount / totalFeedbacksReceived * 100
                : 0;

            // Net Promoter Score (NPS)
            var threeStarCount = await _context.COM_Feedbacks.Where(f => !f.IsDeleted && f.Rating == 3).CountAsync();
            var twoStarCount = await _context.COM_Feedbacks.Where(f => !f.IsDeleted && f.Rating == 2).CountAsync();
            var oneStarCount = await _context.COM_Feedbacks.Where(f => !f.IsDeleted && f.Rating == 1).CountAsync();

            var detractorsCount = oneStarCount + twoStarCount;
            var passivesCount = threeStarCount;

            var feedbacksWithRating = await _context.COM_Feedbacks
                .Where(f => !f.IsDeleted && f.Rating.HasValue)
                .CountAsync();

            var netPromoterScore = feedbacksWithRating > 0
                ? ((decimal)promotersCount / feedbacksWithRating * 100) - ((decimal)detractorsCount / feedbacksWithRating * 100)
                : 0;

            var npsCategory = netPromoterScore > 70 ? "Excellent"
                            : netPromoterScore > 50 ? "Good"
                            : netPromoterScore > 30 ? "Fair"
                            : "Poor";

            // Rating Distribution
            var fiveStarPercentage = feedbacksWithRating > 0 ? (decimal)fiveStarCount / feedbacksWithRating * 100 : 0;
            var fourStarPercentage = feedbacksWithRating > 0 ? (decimal)fourStarCount / feedbacksWithRating * 100 : 0;
            var threeStarPercentage = feedbacksWithRating > 0 ? (decimal)threeStarCount / feedbacksWithRating * 100 : 0;
            var twoStarPercentage = feedbacksWithRating > 0 ? (decimal)twoStarCount / feedbacksWithRating * 100 : 0;
            var oneStarPercentage = feedbacksWithRating > 0 ? (decimal)oneStarCount / feedbacksWithRating * 100 : 0;

            // Engagement Metrics
            // Fix: Attendance rate should be calculated per student per meeting
            var totalMeetings = await _context.ACAD_ClassMeetings.Where(m => !m.IsDeleted).CountAsync();
            var totalStudentsInMeetings = totalMeetings * await _context.IDN_Students.Where(s => !s.IsDeleted).CountAsync();
            var totalAttendances = await _context.ACAD_Attendances.CountAsync();
            var avgAttendanceRate = totalStudentsInMeetings > 0
                ? (decimal)totalAttendances / totalStudentsInMeetings * 100
                : 0;

            var totalAssignments = await _context.ACAD_Assignments.CountAsync();
            var totalSubmissions = await _context.ACAD_Submissions.CountAsync();
            var assignmentSubmissionRate = totalAssignments > 0
                ? (decimal)totalSubmissions / totalAssignments * 100
                : 0;

            var studentParticipationScore = (avgAttendanceRate + assignmentSubmissionRate + feedbackResponseRate) / 3;

            // Satisfaction Trends
            var feedbacksThisMonth = await _context.COM_Feedbacks
                .Where(f => !f.IsDeleted && f.Rating.HasValue && f.CreatedAt >= firstDayOfMonth)
                .AverageAsync(f => (decimal?)f.Rating) ?? 0;

            var feedbacksLastMonth = await _context.COM_Feedbacks
                .Where(f => !f.IsDeleted && f.Rating.HasValue && 
                           f.CreatedAt >= firstDayOfLastMonth &&
                           f.CreatedAt < firstDayOfMonth)
                .AverageAsync(f => (decimal?)f.Rating) ?? 0;

            var satisfactionChangeRate = feedbacksLastMonth > 0
                ? (feedbacksThisMonth - feedbacksLastMonth) / feedbacksLastMonth * 100
                : 0;

            var satisfactionTrend = satisfactionChangeRate > 5 ? "Improving"
                                  : satisfactionChangeRate < -5 ? "Declining"
                                  : "Stable";

            // Complaint/Issue Metrics
            var complaintsCount = await _context.COM_Feedbacks
                .Where(f => !f.IsDeleted && f.Rating.HasValue && f.Rating <= 2)
                .CountAsync();

            var complaintResolutionRate = 75m; // Simplified - would need additional data

            return new EngagementSatisfactionResponse
            {
                OverallFeedbackScore = Math.Round(averageRating, 2),
                AverageRating = Math.Round(averageRating, 2),
                TotalFeedbacksReceived = totalFeedbacksReceived,
                FeedbackResponseRate = Math.Round(feedbackResponseRate, 2),
                StudentSatisfactionScore = Math.Round(studentSatisfactionScore, 2),
                CourseSatisfactionRating = Math.Round(courseSatisfactionRating, 2),
                TeacherSatisfactionRating = Math.Round(teacherSatisfactionRating, 2),
                FacilitySatisfactionRating = Math.Round(facilitySatisfactionRating, 2),
                NetPromoterScore = Math.Round(netPromoterScore, 2),
                PromotersCount = promotersCount,
                PassivesCount = passivesCount,
                DetractorsCount = detractorsCount,
                NPSCategory = npsCategory,
                FiveStarPercentage = Math.Round(fiveStarPercentage, 2),
                FourStarPercentage = Math.Round(fourStarPercentage, 2),
                ThreeStarPercentage = Math.Round(threeStarPercentage, 2),
                TwoStarPercentage = Math.Round(twoStarPercentage, 2),
                OneStarPercentage = Math.Round(oneStarPercentage, 2),
                AverageAttendanceRate = Math.Round(avgAttendanceRate, 2),
                AssignmentSubmissionRate = Math.Round(assignmentSubmissionRate, 2),
                StudentParticipationScore = Math.Round(studentParticipationScore, 2),
                SatisfactionTrend = satisfactionTrend,
                SatisfactionChangeRate = Math.Round(satisfactionChangeRate, 2),
                ComplaintsCount = complaintsCount,
                ComplaintResolutionRate = Math.Round(complaintResolutionRate, 2)
            };
        }

        public async Task<SystemHealthResponse> GetSystemHealthAsync()
        {
            var now = DateTime.Now;
            var today = DateOnly.FromDateTime(now);

            // Course Utilization
            var totalActiveCourses = await _context.ACAD_Courses
                .Where(c => !c.IsDeleted && c.IsActive)
                .CountAsync();

            var coursesWithActiveEnrollments = await _context.ACAD_Enrollments
                .Where(e => !e.IsDeleted)
                .Select(e => e.CourseID)
                .Distinct()
                .CountAsync();

            var courseUtilizationRate = totalActiveCourses > 0
                ? (decimal)coursesWithActiveEnrollments / totalActiveCourses * 100
                : 0;

            var totalEnrollments = await _context.ACAD_Enrollments.Where(e => !e.IsDeleted).CountAsync();
            var avgEnrollmentsPerCourse = totalActiveCourses > 0
                ? (decimal)totalEnrollments / totalActiveCourses
                : 0;

            var underutilizedCoursesCount = await _context.ACAD_Classes
                .Where(c => !c.IsDeleted && c.IsActive && c.Capacity > 0 && c.EnrolledCount < c.Capacity * 0.5)
                .CountAsync();

            // Teacher Workload
            var totalActiveTeachers = await _context.IDN_Teachers
                .Where(t => !t.IsDeleted)
                .CountAsync();

            var totalTeachingHours = await _context.ACAD_ClassMeetings
                .Where(m => !m.IsDeleted)
                .CountAsync() * 2; // Assume 2 hours per meeting

            var avgTeachingHoursPerTeacher = totalActiveTeachers > 0
                ? (decimal)totalTeachingHours / totalActiveTeachers
                : 0;

            var totalClasses = await _context.ACAD_Classes
                .Where(c => !c.IsDeleted && c.IsActive)
                .CountAsync();

            var avgClassesPerTeacher = totalActiveTeachers > 0
                ? (decimal)totalClasses / totalActiveTeachers
                : 0;

            var teacherUtilizationRate = 75m; // Simplified - would need teacher availability data

            var overloadedTeachersCount = (int)(totalActiveTeachers * 0.1); // Assume 10%
            var underutilizedTeachersCount = (int)(totalActiveTeachers * 0.15); // Assume 15%

            // System Load & Performance
            var todayDate = DateOnly.FromDateTime(now);
            var activeClassSessions = await _context.ACAD_ClassMeetings
                .Where(m => !m.IsDeleted && m.Date == todayDate && m.IsStudy)
                .CountAsync();

            var scheduledSessionsToday = await _context.ACAD_ClassMeetings
                .Where(m => !m.IsDeleted && m.Date == todayDate)
                .CountAsync();

            var completedSessionsToday = await _context.ACAD_ClassMeetings
                .Where(m => !m.IsDeleted && m.Date < todayDate)
                .CountAsync();

            var cancelledSessionsToday = 0; // Would need cancellation data

            var systemLoadScore = activeClassSessions > 10 ? 80 : activeClassSessions > 5 ? 50 : 20;

            var peakUsageHours = "9:00 AM - 12:00 PM, 2:00 PM - 5:00 PM"; // Simplified

            var systemHealthStatus = systemLoadScore > 80 ? "Warning"
                                   : systemLoadScore > 60 ? "Healthy"
                                   : "Healthy";

            // Resource Availability
            var availableTeachingSlotsThisWeek = totalActiveTeachers * 40 - (totalClasses * 2); // Simplified
            var availableRoomSlotsThisWeek = await _context.FAC_Rooms
                .Where(r => r.IsActive)
                .CountAsync() * 50; // 50 slots per room per week

            var resourceAvailabilityScore = 80m; // Simplified

            // Data Quality & Completeness
            var totalRecords = await _context.IDN_Students.CountAsync() +
                             await _context.IDN_Teachers.CountAsync() +
                             await _context.ACAD_Courses.CountAsync();

            var completeRecords = totalRecords; // Simplified - would need validation logic
            var dataCompletenessScore = totalRecords > 0
                ? (decimal)completeRecords / totalRecords * 100
                : 100;

            var recordsRequiringAttention = (int)(totalRecords * 0.05); // Assume 5%

            // System Alerts
            var activeAlertsCount = 0;
            var criticalIssuesCount = 0;
            var systemAlerts = new List<string>();

            if (overloadedTeachersCount > totalActiveTeachers * 0.2)
            {
                systemAlerts.Add("High number of overloaded teachers detected");
                activeAlertsCount++;
            }

            if (underutilizedCoursesCount > totalActiveCourses * 0.3)
            {
                systemAlerts.Add("Many courses are underutilized");
                activeAlertsCount++;
            }

            if (dataCompletenessScore < 90)
            {
                systemAlerts.Add("Data completeness below threshold");
                criticalIssuesCount++;
            }

            return new SystemHealthResponse
            {
                TotalActiveCourses = totalActiveCourses,
                CoursesWithActiveEnrollments = coursesWithActiveEnrollments,
                CourseUtilizationRate = Math.Round(courseUtilizationRate, 2),
                AverageEnrollmentsPerCourse = Math.Round(avgEnrollmentsPerCourse, 2),
                UnderutilizedCoursesCount = underutilizedCoursesCount,
                TotalActiveTeachers = totalActiveTeachers,
                AverageTeachingHoursPerTeacher = Math.Round(avgTeachingHoursPerTeacher, 2),
                AverageClassesPerTeacher = Math.Round(avgClassesPerTeacher, 2),
                TeacherUtilizationRate = Math.Round(teacherUtilizationRate, 2),
                OverloadedTeachersCount = overloadedTeachersCount,
                UnderutilizedTeachersCount = underutilizedTeachersCount,
                SystemLoadScore = systemLoadScore,
                PeakUsageHours = peakUsageHours,
                SystemHealthStatus = systemHealthStatus,
                ActiveClassSessions = activeClassSessions,
                ScheduledSessionsToday = scheduledSessionsToday,
                CompletedSessionsToday = completedSessionsToday,
                CancelledSessionsToday = cancelledSessionsToday,
                AvailableTeachingSlotsThisWeek = availableTeachingSlotsThisWeek,
                AvailableRoomSlotsThisWeek = availableRoomSlotsThisWeek,
                ResourceAvailabilityScore = Math.Round(resourceAvailabilityScore, 2),
                DataCompletenessScore = Math.Round(dataCompletenessScore, 2),
                RecordsRequiringAttention = recordsRequiringAttention,
                ActiveAlertsCount = activeAlertsCount,
                CriticalIssuesCount = criticalIssuesCount,
                SystemAlerts = systemAlerts
            };
        }

        // ===== UPDATED OVERALL OVERVIEW =====

        public async Task<OverallOverviewResponse> GetOverallOverviewAsync()
        {
            // Execute all metrics queries sequentially to avoid DbContext concurrency issues
            // Note: EF Core DbContext is not thread-safe and cannot handle parallel operations
            var centerPerformance = await GetCenterPerformanceAsync();
            var growthRetention = await GetGrowthRetentionAsync();
            var revenueFinance = await GetRevenueFinanceAsync();
            var engagementSatisfaction = await GetEngagementSatisfactionAsync();
            var systemHealth = await GetSystemHealthAsync();

            return new OverallOverviewResponse
            {
                CenterPerformance = centerPerformance,
                GrowthRetention = growthRetention,
                RevenueFinance = revenueFinance,
                EngagementSatisfaction = engagementSatisfaction,
                SystemHealth = systemHealth,
                GeneratedAt = DateTime.Now
            };
        }
    }
}

