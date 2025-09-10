using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Course.Responses
{
    public class CourseListItemResponse
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Teacher { get; set; } = null!;
        public string Duration { get; set; } = null!;
        public string Level { get; set; } = null!;
        public decimal Price { get; set; }
        public double Rating { get; set; }
        public int StudentsCount { get; set; }
        public string Image { get; set; } = null!;
        public string Category { get; set; } = null!;
    }
}
