using Domain.Entities.MongoDB;
using Domain.Interfaces.COM;
using Infrastructure.Implementations.Common.Mongo;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Implementations.Repositories.COM;

public class COM_NotificationRepository : ICOM_NotificationRepository
{
    private readonly IMongoCollection<COM_Notification> _collection;

    public COM_NotificationRepository(IMongoClient client, IOptions<MongoNotificationOptions> options)
    {
        var settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
        if (string.IsNullOrWhiteSpace(settings.Database))
        {
            throw new ArgumentException("MongoNotificationOptions.Database is required.");
        }

        if (string.IsNullOrWhiteSpace(settings.Collection))
        {
            throw new ArgumentException("MongoNotificationOptions.Collection is required.");
        }

        var database = client.GetDatabase(settings.Database);
        _collection = database.GetCollection<COM_Notification>(settings.Collection);
    }

    public async Task<IReadOnlyList<COM_Notification>> GetAllAsync()
    {
        var cursor = await _collection
            .Find(Builders<COM_Notification>.Filter.Empty)
            .SortByDescending(x => x.CreatedAt)
            .ToListAsync();

        return cursor;
    }

    public async Task<IReadOnlyList<COM_Notification>> GetByUserAsync(string userId)
    {
        // Normalize userId to a consistent casing for comparison
        var normalizedUserId = userId.ToUpperInvariant();
        var filter = Builders<COM_Notification>.Filter.Eq(x => x.UserId, normalizedUserId);
        var cursor = await _collection
            .Find(filter)
            .SortByDescending(x => x.CreatedAt)
            .ToListAsync();

        return cursor;
    }

    public async Task<COM_Notification?> GetByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return null;
        }

        var filter = Builders<COM_Notification>.Filter.Eq(x => x.Id, objectId.ToString());
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<COM_Notification> CreateAsync(COM_Notification document)
    {
        // Ensure userId is stored in normalized casing
        if (!string.IsNullOrWhiteSpace(document.UserId))
        {
            document.UserId = document.UserId.ToUpperInvariant();
        }
        document.CreatedAt = DateTime.Now;
        await _collection.InsertOneAsync(document);
        return document;
    }

    public async Task<bool> UpdateAsync(COM_Notification document)
    {
        var filter = Builders<COM_Notification>.Filter.Eq(x => x.Id, document.Id);
        var result = await _collection.ReplaceOneAsync(filter, document);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task DeleteAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            return;
        }

        var filter = Builders<COM_Notification>.Filter.Eq(x => x.Id, id);
        await _collection.DeleteOneAsync(filter);
    }
}
