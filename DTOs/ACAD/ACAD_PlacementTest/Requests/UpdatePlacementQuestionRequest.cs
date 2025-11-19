using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_PlacementTest.Requests
{
    public class UpdatePlacementQuestionRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? QuestionUrl { get; set; }
        public Guid SkillTypeID { get; set; }
        public Guid QuestionTypeID { get; set; } // Liên kết với CORE_LookUp
        public int Difficulty { get; set; } = 1;
        public string? QuestionJson { get; set; }
    }
}

