namespace Infrastructure.Implementations.Common.Mongo;

public class MongoNotificationOptions
{
    public const string SectionName = "Mongo:Notification";
    public string? ConnectionString { get; set; }
    public string? Database { get; set; }
    public string Collection { get; set; } = "notifications";
}
