using System;

namespace DTOs.ACAD.ACAD_Class.Responses
{
    public class FeedbackClassResponse
    {
        public Guid CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public Guid? TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public bool HasSubmittedFeedback { get; set; }
    }
}
