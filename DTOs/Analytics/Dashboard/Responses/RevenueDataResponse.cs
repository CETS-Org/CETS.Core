namespace DTOs.Analytics.Dashboard.Responses;

/// <summary>
/// Revenue data point for analytics
/// </summary>
public class RevenueDataPoint
{
    public string Period { get; set; } = null!;
    public decimal Revenue { get; set; }
    public decimal Growth { get; set; }
    public int TransactionCount { get; set; }
}

/// <summary>
/// Complete revenue analytics response
/// </summary>
public class RevenueAnalyticsResponse
{
    public List<RevenueDataPoint> Monthly { get; set; } = new();
    public List<RevenueDataPoint> Quarterly { get; set; } = new();
    public List<RevenueDataPoint> Yearly { get; set; } = new();
    
    public decimal CurrentMonth { get; set; }
    public decimal CurrentQuarter { get; set; }
    public decimal CurrentYear { get; set; }
    public decimal ProjectedNextMonth { get; set; }
}


