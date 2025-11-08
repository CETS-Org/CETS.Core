using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Submission.Requests
{
    public class SubmitWritingSubmissionRequest
    {
        [Required]
        public Guid StudentId { get; set; }

        [Required]
        public Guid AssignmentId { get; set; }

        [Required]
        public string ContentType { get; set; } = null!;

        [Required]
        public string FileName { get; set; } = null!;

        [Required]
        public IFormFile File { get; set; } = null!;
    }
}
