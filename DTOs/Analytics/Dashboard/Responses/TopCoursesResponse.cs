namespace DTOs.Analytics.Dashboard.Responses;

/// <summary>
/// Top course enrollment statistics
/// </summary>
public class TopCourseResponse
{
    public Guid CourseId { get; set; }
    public string CourseName { get; set; } = null!;
    public string CourseCode { get; set; } = null!;
    public string Category { get; set; } = null!;
    
    public int TotalEnrollments { get; set; }
    public int ActiveEnrollments { get; set; }
    public decimal CompletionRate { get; set; }
    public decimal AverageRating { get; set; }
    public decimal Revenue { get; set; }
    
    public string Trend { get; set; } = "stable"; // up, down, stable
    public decimal GrowthRate { get; set; }
}

/// <summary>
/// Course enrollment statistics summary
/// </summary>
public class CourseEnrollmentStatsResponse
{
    public List<TopCourseResponse> TopCourses { get; set; } = new();
    public int TotalCourses { get; set; }
    public int TotalEnrollments { get; set; }
    public decimal AverageEnrollmentPerCourse { get; set; }
}


