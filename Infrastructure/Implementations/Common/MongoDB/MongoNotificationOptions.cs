namespace Infrastructure.Implementations.Common.Mongo;

public class MongoNotificationOptions
{
    public const string SectionName = "Mongo:Notification";
    public string? ConnectionString { get; set; }
    public string Database { get; set; } = null!;
    public string Collection { get; set; } = "notifications";
}
