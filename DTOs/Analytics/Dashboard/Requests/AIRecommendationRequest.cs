namespace DTOs.Analytics.Dashboard.Requests;

/// <summary>
/// Request for AI recommendations with context
/// </summary>
public class AIRecommendationRequest
{
    public List<string> FocusAreas { get; set; } = new(); // revenue, retention, enrollment, operations
    public string Timeframe { get; set; } = "last_6_months"; // last_month, last_3_months, last_6_months, last_year
    public bool IncludeRiskAnalysis { get; set; } = true;
    public bool IncludeOpportunities { get; set; } = true;
}

/// <summary>
/// Request for filtering top courses
/// </summary>
public class TopCoursesRequest
{
    public int TopN { get; set; } = 5;
    public string? CategoryFilter { get; set; }
    public string SortBy { get; set; } = "enrollments"; // enrollments, revenue, rating, growth
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

/// <summary>
/// Request for dropout analysis with filters
/// </summary>
public class DropoutAnalysisRequest
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? AgeGroupFilter { get; set; }
    public string? CourseTypeFilter { get; set; }
    public bool IncludeDemographics { get; set; } = true;
    public bool IncludeRecommendations { get; set; } = true;
}


