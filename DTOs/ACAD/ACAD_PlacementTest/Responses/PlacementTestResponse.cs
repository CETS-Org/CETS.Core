using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_PlacementTest.Responses
{
    public class PlacementTestResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public int DurationMinutes { get; set; }
        public string? StoreUrl { get; set; }
        public List<PlacementQuestionResponse> Questions { get; set; } = new List<PlacementQuestionResponse>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class CreatePlacementTestResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public int DurationMinutes { get; set; }
        public string? UploadUrl { get; set; } // Presigned URL for frontend to upload JSON
        public string? QuestionJson { get; set; } // JSON content for frontend to upload
        public string? StoreUrl { get; set; } // File path for question JSON
        public DateTime CreatedAt { get; set; }
    }
}

