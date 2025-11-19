using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTOs.ACAD.ACAD_PlacementTest.Requests
{
    public class CreatePlacementTestWithQuestionsRequest
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        [Range(1, 600)]
        public int DurationMinutes { get; set; }

        [Required]
        public List<Guid> QuestionIds { get; set; } = new List<Guid>(); // Danh sách ID các câu hỏi được chọn
    }
}

