using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTOs.ACAD.ACAD_Submission.Requests
{
    public class BulkUpdateSubmissionsRequest
    {
        [Required]
        [MinLength(1, ErrorMessage = "At least one submission must be provided")]
        [MaxLength(500, ErrorMessage = "Maximum 500 submissions per request")]
        public List<SubmissionUpdate> Submissions { get; set; } = new List<SubmissionUpdate>();
    }
}


