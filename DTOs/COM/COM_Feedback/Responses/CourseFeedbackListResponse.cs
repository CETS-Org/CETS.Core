using System;

namespace DTOs.COM.COM_Feedback.Responses
{
    public class CourseFeedbackListResponse
    {
        public Guid FeedbackId { get; set; }
        public Guid SubmitterId { get; set; }
        public string SubmitterName { get; set; } = string.Empty;
        public string FeedbackTypeId { get; set; } = string.Empty;
        public string FeedbackTypeName { get; set; } = string.Empty;
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        
        // Course-specific fields
        public string? ContentClarity { get; set; }
        public string? CourseRelevance { get; set; }
        public string? MaterialsQuality { get; set; }
        
        // Teacher-specific fields
        public Guid? TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public string? TeachingEffectiveness { get; set; }
        public string? CommunicationSkills { get; set; }
        public string? TeacherSupportiveness { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}
