using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Course.Responses
{
    public class CourseItemResponse
    {
        public Guid CourseId { get; set; }
        public string CourseCode { get; set; } = default!;
        public string CourseName { get; set; } = default!;
        public List<string> TeacherNames { get; set; } = new();
        public string StatusCode { get; set; } = default!;   
        public string StatusName { get; set; } = default!;
    }
}
