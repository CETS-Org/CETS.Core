using System;

namespace DTOs.ACAD.ACAD_PlacementTest.Responses
{
    public class QuestionTypeResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}

