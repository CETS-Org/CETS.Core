namespace DTOs.ACAD.ACAD_Assignment.Responses
{
    public class QuizAssignmentResponse
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
        public string? QuestionJsonUrl { get; set; } // Presigned URL for frontend to download JSON
        public string? QuestionFilePath { get; set; } // File path for question JSON (for updates)
    }
}