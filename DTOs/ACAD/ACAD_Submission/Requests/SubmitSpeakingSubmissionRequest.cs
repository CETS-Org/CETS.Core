using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.ACAD.ACAD_Submission.Requests
{
    public class SubmitSpeakingSubmissionRequest
    {
        [Required]
        public Guid AssignmentID { get; set; }

        [Required]
        public Guid StudentID { get; set; }

        [Required]
        public string AnswersJsonFilePath { get; set; } = null!; // Path to uploaded answers JSON file
    }
}

