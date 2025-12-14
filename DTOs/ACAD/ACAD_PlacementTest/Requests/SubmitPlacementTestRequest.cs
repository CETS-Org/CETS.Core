using System.ComponentModel.DataAnnotations;

namespace DTOs.ACAD.ACAD_PlacementTest.Requests
{
    public class SubmitPlacementTestRequest
    {
        [Required]
        public Guid PlacementTestId { get; set; }

        [Required]
        public Guid StudentId { get; set; }

        /*[Required]
        public string AnswerData { get; set; } = null!; // JSON string chứa câu trả lời của học sinh
        */
        [Required]
        [Range(0, 900)]
        public decimal Score { get; set; }
    }
}

