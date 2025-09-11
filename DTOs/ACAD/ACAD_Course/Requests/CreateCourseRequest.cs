using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Course.Requests
{
    public class CreateCourseRequest
    {
        public string CourseCode { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public Guid CourseLevelID { get; set; }
        public Guid CourseFormatID { get; set; }
        public string? CourseImageUrl { get; set; }
        public List<string>? CourseObjective { get; set; } = new();
        public Guid CategoryID { get; set; }
        public string? Description { get; set; }
        public decimal StandardPrice { get; set; }
    }
}
