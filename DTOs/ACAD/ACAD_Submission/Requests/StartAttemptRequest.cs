using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.ACAD.ACAD_Submission.Requests
{
    public class StartAttemptRequest
    {
        [Required]
        public Guid AssignmentID { get; set; }

        [Required]
        public Guid StudentID { get; set; }
    }
}

