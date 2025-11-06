namespace DTOs.Analytics.OverallOverview.Responses
{
    /// <summary>
    /// System Health Metrics
    /// Đánh giá hiệu suất hoạt động toàn hệ thống
    /// </summary>
    public class SystemHealthResponse
    {
        // Course Utilization
        /// <summary>
        /// Total active courses
        /// </summary>
        public int TotalActiveCourses { get; set; }

        /// <summary>
        /// Courses with active enrollments
        /// </summary>
        public int CoursesWithActiveEnrollments { get; set; }

        /// <summary>
        /// Course utilization rate (%)
        /// (Courses with enrollments / Total courses) * 100
        /// </summary>
        public decimal CourseUtilizationRate { get; set; }

        /// <summary>
        /// Average enrollments per course
        /// </summary>
        public decimal AverageEnrollmentsPerCourse { get; set; }

        /// <summary>
        /// Underutilized courses count (< 50% capacity)
        /// </summary>
        public int UnderutilizedCoursesCount { get; set; }

        // Teacher Workload
        /// <summary>
        /// Total active teachers
        /// </summary>
        public int TotalActiveTeachers { get; set; }

        /// <summary>
        /// Average teaching hours per teacher
        /// </summary>
        public decimal AverageTeachingHoursPerTeacher { get; set; }

        /// <summary>
        /// Average classes per teacher
        /// </summary>
        public decimal AverageClassesPerTeacher { get; set; }

        /// <summary>
        /// Teacher utilization rate (%)
        /// Based on available vs scheduled hours
        /// </summary>
        public decimal TeacherUtilizationRate { get; set; }

        /// <summary>
        /// Overloaded teachers count (>90% utilization)
        /// </summary>
        public int OverloadedTeachersCount { get; set; }

        /// <summary>
        /// Underutilized teachers count (<50% utilization)
        /// </summary>
        public int UnderutilizedTeachersCount { get; set; }

        // System Load & Performance
        /// <summary>
        /// Current system load score (0-100)
        /// Based on active users, concurrent processes
        /// </summary>
        public decimal SystemLoadScore { get; set; }

        /// <summary>
        /// Peak usage hours indicator
        /// </summary>
        public string PeakUsageHours { get; set; } = string.Empty;

        /// <summary>
        /// System health status: "Healthy", "Warning", "Critical"
        /// </summary>
        public string SystemHealthStatus { get; set; } = string.Empty;

        // Active Sessions
        /// <summary>
        /// Current active class sessions
        /// </summary>
        public int ActiveClassSessions { get; set; }

        /// <summary>
        /// Scheduled sessions today
        /// </summary>
        public int ScheduledSessionsToday { get; set; }

        /// <summary>
        /// Completed sessions today
        /// </summary>
        public int CompletedSessionsToday { get; set; }

        /// <summary>
        /// Cancelled/missed sessions today
        /// </summary>
        public int CancelledSessionsToday { get; set; }

        // Resource Availability
        /// <summary>
        /// Available teaching slots this week
        /// </summary>
        public int AvailableTeachingSlotsThisWeek { get; set; }

        /// <summary>
        /// Available room slots this week
        /// </summary>
        public int AvailableRoomSlotsThisWeek { get; set; }

        /// <summary>
        /// Resource availability score (%)
        /// </summary>
        public decimal ResourceAvailabilityScore { get; set; }

        // Data Quality & Completeness
        /// <summary>
        /// Data completeness score (%)
        /// Percentage of records with complete information
        /// </summary>
        public decimal DataCompletenessScore { get; set; }

        /// <summary>
        /// Records requiring attention/updates
        /// </summary>
        public int RecordsRequiringAttention { get; set; }

        // System Alerts
        /// <summary>
        /// Active system alerts count
        /// </summary>
        public int ActiveAlertsCount { get; set; }

        /// <summary>
        /// Critical issues requiring immediate attention
        /// </summary>
        public int CriticalIssuesCount { get; set; }

        /// <summary>
        /// System alerts summary
        /// </summary>
        public List<string> SystemAlerts { get; set; } = new();
    }
}




