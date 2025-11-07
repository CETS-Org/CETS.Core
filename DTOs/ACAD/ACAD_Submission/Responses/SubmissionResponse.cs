using DTOs.ACAD.ACAD_Assignment.Responses;

namespace DTOs.ACAD.ACAD_Submission.Responses
{
    public class SubmissionResponse
    {
        public Guid Id { get; set; }
        public Guid AssignmentID { get; set; }
        public Guid StudentID { get; set; }
        public string? StudentName { get; set; }
        public string? StudentCode { get; set; }
        public string? StoreUrl { get; set; }
        public string? Content { get; set; }
        public decimal? Score { get; set; }
        public string? Feedback { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UploadUrl { get; set; }

        // public AssignmentResponse? Assignment { get; set; }
    }
}
