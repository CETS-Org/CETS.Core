using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Course.Responses
{
    public class TeachingCourseResponse
    {
        public Guid Id { get; set; }
        public string CourseCode { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public string? CourseImageUrl { get; set; }
        public string CategoryName { get; set; } = null!;
        public string CourseLevel { get; set; } = null!;
        public string FormatName { get; set; } = null!;

        
        public int ActiveClassCount { get; set; }
    }
}
