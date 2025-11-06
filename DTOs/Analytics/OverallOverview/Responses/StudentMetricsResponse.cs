namespace DTOs.Analytics.OverallOverview.Responses
{
    /// <summary>
    /// Student metrics for overall overview dashboard
    /// </summary>
    public class StudentMetricsResponse
    {
        /// <summary>
        /// Total number of active students
        /// </summary>
        public int TotalStudents { get; set; }

        /// <summary>
        /// New students registered this month
        /// </summary>
        public int NewStudentsThisMonth { get; set; }

        /// <summary>
        /// Number of students with active enrollments
        /// </summary>
        public int ActiveStudents { get; set; }

        /// <summary>
        /// Percentage of students who are currently active
        /// </summary>
        public decimal ActiveStudentRate { get; set; }

        /// <summary>
        /// Number of students who have completed at least one course
        /// </summary>
        public int StudentsWithCompletedCourses { get; set; }

        /// <summary>
        /// Average number of enrollments per student
        /// </summary>
        public decimal AverageEnrollmentsPerStudent { get; set; }

        /// <summary>
        /// Student growth rate compared to last month (percentage)
        /// </summary>
        public decimal MonthOverMonthGrowthRate { get; set; }
    }
}





