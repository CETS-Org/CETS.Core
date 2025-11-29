using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Domain.Entities.MongoDB
{
    public class ACAD_ExitSurvey
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("studentId")]
        public string StudentId { get; set; } = null!;

        [BsonElement("academicRequestId")]
        public string? AcademicRequestId { get; set; }

        // Section 1: Reason for dropping out
        [BsonElement("reasonCategory")]
        public string ReasonCategory { get; set; } = null!;

        [BsonElement("reasonDetail")]
        public string ReasonDetail { get; set; } = null!;

        // Section 2: Feedback ratings (1-5 scale)
        [BsonElement("feedback")]
        public ExitSurveyFeedback Feedback { get; set; } = new();

        // Section 3: Future intentions
        [BsonElement("futureIntentions")]
        public ExitSurveyFutureIntentions FutureIntentions { get; set; } = new();

        // Section 4: Free text comments
        [BsonElement("comments")]
        public string Comments { get; set; } = string.Empty;

        // Section 5: Acknowledgement
        [BsonElement("acknowledgesPermanent")]
        public bool AcknowledgesPermanent { get; set; }

        // Metadata
        [BsonElement("completedAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CompletedAt { get; set; }

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; }
    }

    public class ExitSurveyFeedback
    {
        [BsonElement("teacherQuality")]
        public int TeacherQuality { get; set; }

        [BsonElement("classPacing")]
        public int ClassPacing { get; set; }

        [BsonElement("materials")]
        public int Materials { get; set; }

        [BsonElement("staffService")]
        public int StaffService { get; set; }

        [BsonElement("schedule")]
        public int Schedule { get; set; }

        [BsonElement("facilities")]
        public int Facilities { get; set; }
    }

    public class ExitSurveyFutureIntentions
    {
        [BsonElement("wouldReturnInFuture")]
        public bool WouldReturnInFuture { get; set; }

        [BsonElement("wouldRecommendToOthers")]
        public bool WouldRecommendToOthers { get; set; }
    }
}

