using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Course.Responses
{
    public class CourseResponse
    {
        public Guid Id { get; set; }
        public string CourseCode { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public string? CourseImageUrl { get; set; }
        public List<string>? CourseObjective { get; set; } = new();
        public string? Description { get; set; }
        public decimal StandardPrice { get; set; }
        public bool IsActive { get; set; }
        
        // IDs needed for edit forms
        public Guid CourseLevelID { get; set; }
        public Guid CourseFormatID { get; set; }
        public Guid CategoryID { get; set; }
    }
}
