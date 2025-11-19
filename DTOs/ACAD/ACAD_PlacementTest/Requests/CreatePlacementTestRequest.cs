using System.ComponentModel.DataAnnotations;

namespace DTOs.ACAD.ACAD_PlacementTest.Requests
{
    public class CreatePlacementTestRequest
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        [Range(1, 600)]
        public int DurationMinutes { get; set; }

        [Required]
        public string QuestionJson { get; set; } = null!; // JSON string chứa câu hỏi từ frontend
    }
}

