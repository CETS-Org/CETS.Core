using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_AcademicRequest.Requests
{
    public class CreateDropoutRequest
    {
        [Required]
        public Guid StudentID { get; set; }

        [Required]
        public Guid RequestTypeID { get; set; }

        [Required]
        public DateOnly EffectiveDate { get; set; }

        [Required]
        [StringLength(50)]
        public string ReasonCategory { get; set; } = null!;

        [Required]
        [StringLength(1000)]
        public string ReasonDetail { get; set; } = null!;

        [StringLength(500)]
        public string? AttachmentUrl { get; set; }

        public bool CompletedExitSurvey { get; set; }

        [StringLength(100)]
        public string? ExitSurveyId { get; set; }

        // Related enrollment (required for status tracking)
        [Required]
        public Guid EnrollmentID { get; set; }
    }
}

