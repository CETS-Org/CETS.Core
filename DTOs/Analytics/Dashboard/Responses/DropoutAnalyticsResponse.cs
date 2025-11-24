namespace DTOs.Analytics.Dashboard.Responses;

/// <summary>
/// Dropout trend data point
/// </summary>
public class DropoutTrendPoint
{
    public string Period { get; set; } = null!;
    public int TotalStudents { get; set; }
    public int DroppedOut { get; set; }
    public decimal DropoutRate { get; set; }
    public decimal RetentionRate { get; set; }
}

/// <summary>
/// Dropout reason statistics
/// </summary>
public class DropoutReason
{
    public string Reason { get; set; } = null!;
    public int Count { get; set; }
    public decimal Percentage { get; set; }
}

/// <summary>
/// Demographic dropout analysis
/// </summary>
public class DemographicDropoutAnalysis
{
    public string AgeGroup { get; set; } = null!;
    public string CourseType { get; set; } = null!;
    public string EnrollmentDuration { get; set; } = null!;
    public int DropoutCount { get; set; }
    public int TotalStudents { get; set; }
    public decimal DropoutRate { get; set; }
}

/// <summary>
/// Dropout by class statistics
/// </summary>
public class DropoutByClass
{
    public string ClassId { get; set; } = null!;
    public string ClassName { get; set; } = null!;
    public string CourseName { get; set; } = null!;
    public int TotalStudents { get; set; }
    public int DroppedOut { get; set; }
    public decimal DropoutRate { get; set; }
    public DateOnly StartDate { get; set; }
    public string Status { get; set; } = null!;
}

/// <summary>
/// Complete student dropout analytics response
/// </summary>
public class StudentDropoutAnalyticsResponse
{
    public decimal OverallDropoutRate { get; set; }
    public List<DropoutTrendPoint> DropoutTrend { get; set; } = new();
    public List<DropoutReason> TopReasons { get; set; } = new();
    public List<DemographicDropoutAnalysis> DemographicAnalysis { get; set; } = new();
    public List<DropoutByClass> DropoutByClass { get; set; } = new();
    public int HighRiskStudents { get; set; }
    public int AverageTimeToDropout { get; set; } // in days
    public List<string> Recommendations { get; set; } = new();
}


