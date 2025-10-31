using System;

namespace DTOs.ACAD.ACAD_Submission.Responses
{
    public class SubmissionUpdateResult
    {
        public Guid SubmissionId { get; set; }
        public string Status { get; set; } = string.Empty; // "success" or "failed"
        public UpdateDetails? Updates { get; set; }
        public string? Error { get; set; }
    }
}


