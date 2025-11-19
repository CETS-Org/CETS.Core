using System;
using System.Collections.Generic;

namespace DTOs.ACAD.ACAD_PlacementTest.Requests
{
    public class UpdatePlacementTestRequest
    {
        public string Title { get; set; } = null!; 

        public int DurationMinutes { get; set; } 

        public List<Guid> QuestionIds { get; set; } = new List<Guid>(); // Danh sách ID các câu hỏi

        public bool IsDeleted { get; set; }
    }
}
