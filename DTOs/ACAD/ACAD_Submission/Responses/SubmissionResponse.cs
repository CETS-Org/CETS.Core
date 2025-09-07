using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Submission.Responses
{
    public class SubmissionResponse
    {
        public Guid Id { get; set; }
        public Guid AssignmentID { get; set; }
        public Guid StudentID { get; set; }
        public string? FileUrl { get; set; }
        public string? Content { get; set; }
        public decimal? Score { get; set; }
        public string? Feedback { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
