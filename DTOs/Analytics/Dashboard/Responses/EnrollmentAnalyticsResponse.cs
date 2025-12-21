namespace DTOs.Analytics.Dashboard.Responses;

/// <summary>
/// Enrollment data point by period
/// </summary>
public class EnrollmentTrendPoint
{
    public string Period { get; set; } = null!;
    public int TotalEnrollments { get; set; }
    public int ActiveEnrollments { get; set; }
    public int CompletedEnrollments { get; set; }
    public int DroppedEnrollments { get; set; }
    public decimal GrowthRate { get; set; }
}

/// <summary>
/// Enrollment by course analysis
/// </summary>
public class EnrollmentByCourse
{
    public string CourseId { get; set; } = null!;
    public string CourseName { get; set; } = null!;
    public string CourseCode { get; set; } = null!;
    public string Category { get; set; } = null!;
    public int TotalEnrollments { get; set; }
    public int ActiveEnrollments { get; set; }
    public decimal GrowthRate { get; set; }
    public string Trend { get; set; } = null!; // "up", "down", "stable"
}

/// <summary>
/// Enrollment by course analysis (aggregated from enrollments)
/// </summary>
public class EnrollmentByCourseAggregated
{
    public string CourseId { get; set; } = null!;
    public string CourseName { get; set; } = null!;
    public string? CourseCode { get; set; }
    public int TotalEnrollments { get; set; }
    public int ActiveEnrollments { get; set; }
    public int CompletedEnrollments { get; set; }
    public int DroppedEnrollments { get; set; }
    public int NumberOfClasses { get; set; }
}

/// <summary>
/// Complete student enrollment analytics response
/// </summary>
public class StudentEnrollmentAnalyticsResponse
{
    public int TotalEnrollments { get; set; }
    public int ActiveEnrollments { get; set; }
    public int CompletedEnrollments { get; set; }
    public int DroppedEnrollments { get; set; }
    public decimal MonthOverMonthGrowth { get; set; }
    public decimal QuarterOverQuarterGrowth { get; set; }
    public List<EnrollmentTrendPoint> MonthlyTrend { get; set; } = new();
    public List<EnrollmentTrendPoint> QuarterlyTrend { get; set; } = new();
    public List<EnrollmentByCourse> TopGrowingCourses { get; set; } = new();
    public List<EnrollmentByCourseAggregated> EnrollmentByCourse { get; set; } = new();
    public List<string> Insights { get; set; } = new();
}

