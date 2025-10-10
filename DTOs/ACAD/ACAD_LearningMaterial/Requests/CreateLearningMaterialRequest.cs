using System.ComponentModel.DataAnnotations;

namespace DTOs.ACAD.ACAD_LearningMaterial.Requests
{
    public class CreateLearningMaterialRequest
    {
        [Required]
        public Guid ClassMeetingID { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; } = null!;

        [Required]
        public string ContentType { get; set; } = null!;

        [Required]
        public string FileName { get; set; } = null!;
    }
}
