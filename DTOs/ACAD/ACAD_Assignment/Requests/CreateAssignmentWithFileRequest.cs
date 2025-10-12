using System.ComponentModel.DataAnnotations;

namespace DTOs.ACAD.ACAD_Assignment.Requests
{
    public class CreateAssignmentWithFileRequest
    {
        [Required]
        public Guid ClassMeetingId { get; set; }

        [Required]
        public Guid TeacherId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public string ContentType { get; set; } = null!;

        [Required]
        public string FileName { get; set; } = null!;
    }
}



