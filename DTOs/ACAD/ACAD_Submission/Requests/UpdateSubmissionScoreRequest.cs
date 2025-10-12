using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Submission.Requests
{
    public class UpdateSubmissionScoreRequest
    {
        [Required]
        public Guid SubmissionId { get; set; }

        [Required]
        [Range(0, 10, ErrorMessage = "Score must be between 0 and 10")]
        public decimal Score { get; set; }
    }
}

