namespace DTOs.ACAD.ACAD_Assignment.Responses
{
    public class SpeakingAssignmentResponse
    {
        public Guid Id { get; set; }
        public Guid ClassMeetingId { get; set; }
        public Guid TeacherId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? SkillID { get; set; }
        public string? SkillName { get; set; }
        public string? AudioUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string? UploadUrl { get; set; } // Presigned URL for frontend to upload JSON
        public string? QuestionJson { get; set; } // JSON content for frontend to upload
        public string? AudioUploadUrl { get; set; } // Presigned URL for frontend to upload audio
        public string? VideoUploadUrl { get; set; } // Presigned URL for frontend to upload video
        public string? QuestionJsonUrl { get; set; } // Presigned URL for frontend to download JSON
    }
}