namespace DTOs.Analytics.OverallOverview.Responses
{
    /// <summary>
    /// Growth & Retention Metrics
    /// Theo dõi tăng trưởng số lượng học viên, tỉ lệ rời bỏ, reactivation
    /// </summary>
    public class GrowthRetentionResponse
    {
        // Enrollment Trends
        /// <summary>
        /// Total active students currently enrolled
        /// </summary>
        public int TotalActiveStudents { get; set; }

        /// <summary>
        /// New students enrolled this month
        /// </summary>
        public int NewStudentsThisMonth { get; set; }

        /// <summary>
        /// New students enrolled last month
        /// </summary>
        public int NewStudentsLastMonth { get; set; }

        /// <summary>
        /// Month-over-month growth rate (%)
        /// </summary>
        public decimal MonthOverMonthGrowthRate { get; set; }

        /// <summary>
        /// Total enrollments this month
        /// </summary>
        public int TotalEnrollmentsThisMonth { get; set; }

        /// <summary>
        /// Enrollment trend: "Increasing", "Stable", "Decreasing"
        /// </summary>
        public string EnrollmentTrend { get; set; } = string.Empty;

        // Retention Metrics
        /// <summary>
        /// Student retention rate (%)
        /// Students who continue after completing a course
        /// </summary>
        public decimal RetentionRate { get; set; }

        /// <summary>
        /// Number of students retained (re-enrolled)
        /// </summary>
        public int StudentsRetained { get; set; }

        /// <summary>
        /// Students who completed courses in last 3 months
        /// </summary>
        public int StudentsCompletedLast3Months { get; set; }

        // Churn Metrics
        /// <summary>
        /// Churn rate - students who left/dropped out (%)
        /// </summary>
        public decimal ChurnRate { get; set; }

        /// <summary>
        /// Number of students churned this month
        /// </summary>
        public int StudentsChurnedThisMonth { get; set; }

        /// <summary>
        /// Number of students churned last month
        /// </summary>
        public int StudentsChurnedLastMonth { get; set; }

        /// <summary>
        /// Churn trend: "Improving" (decreasing), "Stable", "Worsening" (increasing)
        /// </summary>
        public string ChurnTrend { get; set; } = string.Empty;

        // Reactivation
        /// <summary>
        /// Reactivation rate - previously inactive students who re-enrolled (%)
        /// </summary>
        public decimal ReactivationRate { get; set; }

        /// <summary>
        /// Number of reactivated students this month
        /// </summary>
        public int ReactivatedStudentsThisMonth { get; set; }

        /// <summary>
        /// Total inactive students who could be reactivated
        /// </summary>
        public int PotentialReactivationPool { get; set; }

        // Lifetime Value Indicators
        /// <summary>
        /// Average number of courses per student
        /// </summary>
        public decimal AverageCoursesPerStudent { get; set; }

        /// <summary>
        /// Average student lifetime in months
        /// </summary>
        public decimal AverageStudentLifetimeMonths { get; set; }

        /// <summary>
        /// Student lifetime value estimation
        /// </summary>
        public decimal AverageStudentLifetimeValue { get; set; }
    }
}




