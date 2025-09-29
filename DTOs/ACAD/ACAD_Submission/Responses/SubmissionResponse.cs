using DTOs.ACAD.ACAD_Assignment.Responses;

namespace DTOs.ACAD.ACAD_Submission.Responses
{
    public class SubmissionResponse
    {
        public Guid Id { get; set; }
        public Guid AssignmentID { get; set; }
        public Guid StudentID { get; set; }
        public string? StoreUrl { get; set; }
        public string? Content { get; set; }
        public decimal? Score { get; set; }
        public string? Feedback { get; set; }
        public DateTime CreatedAt { get; set; }

        public AssignmentResponse? Assignment { get; set; }
    }
}
