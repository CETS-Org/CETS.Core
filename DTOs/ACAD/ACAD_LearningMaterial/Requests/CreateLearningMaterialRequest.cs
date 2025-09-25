using System.ComponentModel.DataAnnotations;

namespace DTOs.ACAD.ACAD_LearningMaterial.Requests
{
    public class CreateLearningMaterialRequest
    {
        public Guid? ClassID { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; } = null!;

        [Required]
        public string ContentType { get; set; } = null!;

        [Required]
        public string FileName { get; set; } = null!;
    }
}
