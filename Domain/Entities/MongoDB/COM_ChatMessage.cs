using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MongoDB
{
    public class COM_ChatMessage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("roomId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string RoomId { get; set; } = null!;

        [BsonElement("senderId")]
        public string SenderId { get; set; } = null!;

        [BsonElement("content")]
        public string Content { get; set; } = null!;

        // "text", "image", "file", "assignment_link"
        [BsonElement("type")]
        public string Type { get; set; } = "text";

        // Lưu AssignmentId, LinkUrl, v.v.
        [BsonElement("metadata")]
        public Dictionary<string, string>? Metadata { get; set; }

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; }
    }
}
