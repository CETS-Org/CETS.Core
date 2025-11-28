using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MongoDB
{
    public class COM_ChatRoom
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("name")]
        public string? Name { get; set; } // Null nếu là chat 1v1

        [BsonElement("type")]
        public string Type { get; set; } = "private"; // "private" hoặc "group"

        [BsonElement("memberIds")]
        public List<string> MemberIds { get; set; } = new();

        [BsonElement("lastMessageAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime LastMessageAt { get; set; }

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; }
    }
}
