namespace DTOs.ACAD.ACAD_ExitSurvey.Responses;

public class ExitSurveyResponse
{
    public string Id { get; set; } = null!;
    public string StudentId { get; set; } = null!;
    public string? AcademicRequestId { get; set; }
    
    // Section 1: Reason for dropping out
    public string ReasonCategory { get; set; } = null!;
    public string ReasonDetail { get; set; } = null!;
    
    // Section 2: Feedback ratings (1-5 scale)
    public ExitSurveyFeedbackDto Feedback { get; set; } = new();
    
    // Section 3: Future intentions
    public ExitSurveyFutureIntentionsDto FutureIntentions { get; set; } = new();
    
    // Section 4: Free text comments
    public string Comments { get; set; } = string.Empty;
    
    // Section 5: Acknowledgement
    public bool AcknowledgesPermanent { get; set; }
    
    // Metadata
    public DateTime CompletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ExitSurveyFeedbackDto
{
    public int TeacherQuality { get; set; }
    public int ClassPacing { get; set; }
    public int Materials { get; set; }
    public int StaffService { get; set; }
    public int Schedule { get; set; }
    public int Facilities { get; set; }
}

public class ExitSurveyFutureIntentionsDto
{
    public bool WouldReturnInFuture { get; set; }
    public bool WouldRecommendToOthers { get; set; }
}

public class ExitSurveyAnalyticsResponse
{
    public int TotalSurveys { get; set; }
    public Dictionary<string, int> ReasonCategoryStatistics { get; set; } = new();
    public Dictionary<string, double> AverageFeedbackRatings { get; set; } = new();
    public int SurveysThisMonth { get; set; }
    public int SurveysThisYear { get; set; }
}

