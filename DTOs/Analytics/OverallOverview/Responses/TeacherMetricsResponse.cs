namespace DTOs.Analytics.OverallOverview.Responses
{
    /// <summary>
    /// Teacher metrics for overall overview dashboard
    /// </summary>
    public class TeacherMetricsResponse
    {
        /// <summary>
        /// Total number of teachers
        /// </summary>
        public int TotalTeachers { get; set; }

        /// <summary>
        /// Number of teachers currently assigned to active classes
        /// </summary>
        public int ActiveTeachers { get; set; }

        /// <summary>
        /// Number of teachers with valid contracts
        /// </summary>
        public int TeachersWithValidContracts { get; set; }

        /// <summary>
        /// Number of contracts expiring within 30 days
        /// </summary>
        public int ContractsExpiringSoon { get; set; }

        /// <summary>
        /// Average years of experience across all teachers
        /// </summary>
        public decimal AverageYearsExperience { get; set; }

        /// <summary>
        /// Average rating across all teachers
        /// </summary>
        public decimal AverageTeacherRating { get; set; }

        /// <summary>
        /// Total number of classes being taught
        /// </summary>
        public int TotalClassesTeaching { get; set; }

        /// <summary>
        /// Average number of classes per teacher
        /// </summary>
        public decimal AverageClassesPerTeacher { get; set; }

        /// <summary>
        /// Total teaching hours across all teachers (estimated)
        /// </summary>
        public int TotalTeachingHours { get; set; }
    }
}







