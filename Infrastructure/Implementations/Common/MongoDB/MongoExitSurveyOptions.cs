namespace Infrastructure.Implementations.Common.Mongo;

public class MongoExitSurveyOptions
{
    public const string SectionName = "Mongo:ExitSurvey";
    public string? ConnectionString { get; set; }
    public string? Database { get; set; }
    public string Collection { get; set; } = "exitSurveys";
}

