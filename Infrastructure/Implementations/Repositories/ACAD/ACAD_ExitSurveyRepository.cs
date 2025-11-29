using Domain.Entities.MongoDB;
using Domain.Interfaces.ACAD;
using Infrastructure.Implementations.Common.Mongo;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Implementations.Repositories.ACAD;

public class ACAD_ExitSurveyRepository : IACAD_ExitSurveyRepository
{
    private readonly IMongoCollection<ACAD_ExitSurvey> _collection;

    public ACAD_ExitSurveyRepository(IMongoClient client, IOptions<MongoExitSurveyOptions> options)
    {
        var settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
        if (string.IsNullOrWhiteSpace(settings.Database))
        {
            throw new ArgumentException("MongoExitSurveyOptions.Database is required.");
        }

        if (string.IsNullOrWhiteSpace(settings.Collection))
        {
            throw new ArgumentException("MongoExitSurveyOptions.Collection is required.");
        }

        var database = client.GetDatabase(settings.Database);
        _collection = database.GetCollection<ACAD_ExitSurvey>(settings.Collection);
    }

    public async Task<IReadOnlyList<ACAD_ExitSurvey>> GetAllAsync()
    {
        var cursor = await _collection
            .Find(Builders<ACAD_ExitSurvey>.Filter.Empty)
            .SortByDescending(x => x.CreatedAt)
            .ToListAsync();

        return cursor;
    }

    public async Task<IReadOnlyList<ACAD_ExitSurvey>> GetByStudentAsync(string studentId)
    {
        var normalizedStudentId = studentId.ToUpperInvariant();
        var filter = Builders<ACAD_ExitSurvey>.Filter.Eq(x => x.StudentId, normalizedStudentId);
        var cursor = await _collection
            .Find(filter)
            .SortByDescending(x => x.CreatedAt)
            .ToListAsync();

        return cursor;
    }

    public async Task<ACAD_ExitSurvey?> GetByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
        {
            return null;
        }

        var filter = Builders<ACAD_ExitSurvey>.Filter.Eq(x => x.Id, objectId.ToString());
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<ACAD_ExitSurvey?> GetByAcademicRequestIdAsync(string academicRequestId)
    {
        var normalizedRequestId = academicRequestId.ToUpperInvariant();
        var filter = Builders<ACAD_ExitSurvey>.Filter.Eq(x => x.AcademicRequestId, normalizedRequestId);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<ACAD_ExitSurvey> CreateAsync(ACAD_ExitSurvey document)
    {
        // Ensure IDs are stored in normalized casing
        if (!string.IsNullOrWhiteSpace(document.StudentId))
        {
            document.StudentId = document.StudentId.ToUpperInvariant();
        }
        if (!string.IsNullOrWhiteSpace(document.AcademicRequestId))
        {
            document.AcademicRequestId = document.AcademicRequestId.ToUpperInvariant();
        }
        
        document.CreatedAt = DateTime.UtcNow;
        await _collection.InsertOneAsync(document);
        return document;
    }

    public async Task<bool> UpdateAsync(ACAD_ExitSurvey document)
    {
        var filter = Builders<ACAD_ExitSurvey>.Filter.Eq(x => x.Id, document.Id);
        var result = await _collection.ReplaceOneAsync(filter, document);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task DeleteAsync(string id)
    {
        if (!ObjectId.TryParse(id, out _))
        {
            return;
        }

        var filter = Builders<ACAD_ExitSurvey>.Filter.Eq(x => x.Id, id);
        await _collection.DeleteOneAsync(filter);
    }

    // Analytics methods
    public async Task<Dictionary<string, int>> GetReasonCategoryStatisticsAsync()
    {
        var pipeline = new[]
        {
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", "$reasonCategory" },
                { "count", new BsonDocument("$sum", 1) }
            })
        };

        var results = await _collection.Aggregate<BsonDocument>(pipeline).ToListAsync();
        return results.ToDictionary(
            doc => doc["_id"].AsString,
            doc => doc["count"].AsInt32
        );
    }

    public async Task<Dictionary<string, double>> GetAverageFeedbackRatingsAsync()
    {
        var pipeline = new[]
        {
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", BsonNull.Value },
                { "avgTeacherQuality", new BsonDocument("$avg", "$feedback.teacherQuality") },
                { "avgClassPacing", new BsonDocument("$avg", "$feedback.classPacing") },
                { "avgMaterials", new BsonDocument("$avg", "$feedback.materials") },
                { "avgStaffService", new BsonDocument("$avg", "$feedback.staffService") },
                { "avgSchedule", new BsonDocument("$avg", "$feedback.schedule") },
                { "avgFacilities", new BsonDocument("$avg", "$feedback.facilities") }
            })
        };

        var results = await _collection.Aggregate<BsonDocument>(pipeline).ToListAsync();
        var result = results.FirstOrDefault();
        
        if (result == null)
        {
            return new Dictionary<string, double>();
        }

        return new Dictionary<string, double>
        {
            { "teacherQuality", result["avgTeacherQuality"].ToDouble() },
            { "classPacing", result["avgClassPacing"].ToDouble() },
            { "materials", result["avgMaterials"].ToDouble() },
            { "staffService", result["avgStaffService"].ToDouble() },
            { "schedule", result["avgSchedule"].ToDouble() },
            { "facilities", result["avgFacilities"].ToDouble() }
        };
    }

    public async Task<int> GetTotalSurveysCountAsync()
    {
        return (int)await _collection.CountDocumentsAsync(Builders<ACAD_ExitSurvey>.Filter.Empty);
    }

    public async Task<int> GetSurveysCountByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var filter = Builders<ACAD_ExitSurvey>.Filter.And(
            Builders<ACAD_ExitSurvey>.Filter.Gte(x => x.CreatedAt, startDate),
            Builders<ACAD_ExitSurvey>.Filter.Lte(x => x.CreatedAt, endDate)
        );
        
        return (int)await _collection.CountDocumentsAsync(filter);
    }
}

