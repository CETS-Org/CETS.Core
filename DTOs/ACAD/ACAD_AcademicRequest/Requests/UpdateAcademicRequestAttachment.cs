using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.ACAD.ACAD_AcademicRequest.Requests
{
    public class UpdateAcademicRequestAttachment
    {
        [Required]
        public Guid RequestID { get; set; }

        [Required]
        [StringLength(500)]
        public string AttachmentUrl { get; set; } = string.Empty;

        public string? AdditionalNotes { get; set; }
    }
}

