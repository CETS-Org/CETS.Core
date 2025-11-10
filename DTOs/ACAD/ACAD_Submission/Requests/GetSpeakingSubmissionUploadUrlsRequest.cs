using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTOs.ACAD.ACAD_Submission.Requests
{
    public class GetSpeakingSubmissionUploadUrlsRequest
    {
        [Required]
        public Guid AssignmentID { get; set; }

        [Required]
        public Guid StudentID { get; set; }

        // List of question IDs that have audio files to upload
        public List<string>? AudioQuestionIds { get; set; }
    }
}

