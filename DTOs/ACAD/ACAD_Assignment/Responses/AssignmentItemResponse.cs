using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Assignment.Responses
{
    public class StudentAssignmentResponse
    {
        public Guid AssignmentId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueAt { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public decimal? Score { get; set; }
        public string? Feedback { get; set; }
        public string SubmissionStatus { get; set; } = "NOT_SUBMITTED";
    }
}
