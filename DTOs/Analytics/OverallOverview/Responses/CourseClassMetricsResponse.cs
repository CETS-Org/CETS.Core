namespace DTOs.Analytics.OverallOverview.Responses
{
    /// <summary>
    /// Course and class metrics for overall overview dashboard
    /// </summary>
    public class CourseClassMetricsResponse
    {
        /// <summary>
        /// Total number of active courses
        /// </summary>
        public int TotalActiveCourses { get; set; }

        /// <summary>
        /// Total number of all courses (active and inactive)
        /// </summary>
        public int TotalCourses { get; set; }

        /// <summary>
        /// Total number of classes
        /// </summary>
        public int TotalClasses { get; set; }

        /// <summary>
        /// Number of classes currently in progress
        /// </summary>
        public int OngoingClasses { get; set; }

        /// <summary>
        /// Number of classes scheduled for the future
        /// </summary>
        public int UpcomingClasses { get; set; }

        /// <summary>
        /// Number of completed classes
        /// </summary>
        public int CompletedClasses { get; set; }

        /// <summary>
        /// Average fill rate across all classes (percentage)
        /// </summary>
        public decimal AverageClassFillRate { get; set; }

        /// <summary>
        /// Total number of enrollments
        /// </summary>
        public int TotalEnrollments { get; set; }

        /// <summary>
        /// Number of active enrollments
        /// </summary>
        public int ActiveEnrollments { get; set; }

        /// <summary>
        /// Number of completed enrollments
        /// </summary>
        public int CompletedEnrollments { get; set; }

        /// <summary>
        /// Number of dropped/cancelled enrollments
        /// </summary>
        public int DroppedEnrollments { get; set; }

        /// <summary>
        /// Enrollment completion rate (percentage)
        /// </summary>
        public decimal EnrollmentCompletionRate { get; set; }

        /// <summary>
        /// Enrollment dropout rate (percentage)
        /// </summary>
        public decimal EnrollmentDropoutRate { get; set; }
    }
}



















