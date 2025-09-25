using System.ComponentModel.DataAnnotations;

namespace DTOs.ACAD.ACAD_LearningMaterial.Requests
{
    public class UpdateLearningMaterialRequest
    {
        public Guid Id { get; set; }

        public Guid? ClassID { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; } = null!;
    }
}
