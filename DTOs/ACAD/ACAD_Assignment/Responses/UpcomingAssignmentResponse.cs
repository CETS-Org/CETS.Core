namespace DTOs.ACAD.ACAD_Assignment.Responses
{
    public class UpcomingAssignmentResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime? DueAt { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public Guid ClassId { get; set; }
        public Guid ClassMeetingId { get; set; }
        public int SessionNumber { get; set; }
        public bool HasSubmission { get; set; }
        public bool IsOverdue { get; set; }
    }
}

