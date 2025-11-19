using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_PlacementTest.Requests
{
    public class CreatePlacementQuestionRequest
    {
        public string Title { get; set; } = null!;
        public string? QuestionUrl { get; set; }
        public Guid SkillTypeID { get; set; }
        public Guid QuestionTypeID { get; set; } // Liên kết với CORE_LookUp
        public int Difficulty { get; set; } = 1; // 1: câu hỏi đơn, 2: đoạn văn/audio ngắn, 3: đoạn văn/audio dài
        public string? QuestionJson { get; set; } // JSON content để upload
    }
}

