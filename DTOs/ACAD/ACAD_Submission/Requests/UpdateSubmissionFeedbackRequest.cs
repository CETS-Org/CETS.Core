using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Submission.Requests
{
    public class UpdateSubmissionFeedbackRequest
    {
        [Required]
        public Guid SubmissionId { get; set; }

        [Required]
        [MaxLength(1000, ErrorMessage = "Feedback cannot exceed 1000 characters")]
        public string Feedback { get; set; } = string.Empty;
    }
}

