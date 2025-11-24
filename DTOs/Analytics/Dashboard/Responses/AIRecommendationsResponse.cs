namespace DTOs.Analytics.Dashboard.Responses;

/// <summary>
/// AI-generated recommendation for strategic decisions
/// </summary>
public class AIRecommendation
{
    public string Id { get; set; } = null!;
    public string Category { get; set; } = null!; // revenue, enrollment, retention, operations, marketing
    public string Priority { get; set; } = null!; // high, medium, low
    
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Impact { get; set; } = null!;
    
    public List<string> ActionItems { get; set; } = new();
    
    public EstimatedImpact EstimatedImpact { get; set; } = new();
    
    public int Confidence { get; set; } // 0-100
    public DateTime GeneratedAt { get; set; }
}

/// <summary>
/// Estimated impact of recommendation
/// </summary>
public class EstimatedImpact
{
    public decimal? Revenue { get; set; }
    public int? Enrollments { get; set; }
    public decimal? Retention { get; set; }
}

/// <summary>
/// Complete AI analysis response
/// </summary>
public class AIAnalysisResponse
{
    public List<AIRecommendation> Recommendations { get; set; } = new();
    public string Summary { get; set; } = null!;
    public List<string> KeyInsights { get; set; } = new();
    public List<string> RiskFactors { get; set; } = new();
    public List<string> Opportunities { get; set; } = new();
    public DateTime GeneratedAt { get; set; }
}


