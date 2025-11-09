using System.ComponentModel.DataAnnotations;

namespace DTOs.ACAD.ACAD_Assignment.Requests
{
    public class CreateSpeakingAssignmentRequest
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
        public string QuestionJson { get; set; } = null!; // JSON string from frontend

        public Guid? SkillID { get; set; }

        [Required]
        [StringLength(50)]
        public string AssignmentType { get; set; } = "homework"; // "quiz" or "homework"
    }
}