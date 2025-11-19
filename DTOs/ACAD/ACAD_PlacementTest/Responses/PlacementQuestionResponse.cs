using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_PlacementTest.Responses
{
    public class PlacementQuestionResponse
    {
        public Guid Id { get; set; }
        public string SkillType { get; set; } = null!; // "Reading" hoặc "Listening"
        public Guid SkillTypeID { get; set; }

        public string QuestionType { get; set; } = null!; // Tên từ CORE_LookUp (MCQ, TrueFalse, FillGap, Passage, Audio)
        public Guid QuestionTypeID { get; set; } // ID từ CORE_LookUp

        [StringLength(100)]
        public string Title { get; set; } = null!; // Tên câu hỏi hoặc passage title

        public string? QuestionUrl { get; set; } // Link JSON trên cloud

        public int Difficulty { get; set; } = 1; // 1: câu hỏi đơn, 2: ngắn, 3: dài

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
