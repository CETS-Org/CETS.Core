using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Assignment.Requests
{
    public class UpdateAssignmentRequest
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueAt { get; set; }
        public Guid? SkillID { get; set; }
        public string? AssignmentType { get; set; } // "quiz" or "homework"
        public string? StoreUrl { get; set; }
        public string? ContentType { get; set; }
        public string? FileName { get; set; }
        public string? QuestionUrl { get; set; } // File path for question JSON 
    }
}
