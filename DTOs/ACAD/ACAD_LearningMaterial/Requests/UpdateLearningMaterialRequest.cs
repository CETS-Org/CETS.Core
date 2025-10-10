using System.ComponentModel.DataAnnotations;

namespace DTOs.ACAD.ACAD_LearningMaterial.Requests
{
    public class UpdateLearningMaterialRequest
    {
        public Guid Id { get; set; }

        public Guid? ClassMeetingID { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; } = null!;

        // Optional file update properties
        public string? ContentType { get; set; }
        public string? FileName { get; set; }
    }
}
