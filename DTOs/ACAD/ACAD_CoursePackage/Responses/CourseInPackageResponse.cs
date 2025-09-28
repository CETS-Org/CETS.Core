using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_CoursePackage.Responses
{
    public class CourseInPackageResponse
    {
        public Guid CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int Sequence { get; set; }
        public decimal StandardPrice { get; set; }
        public string? Description { get; set; }
        public string? Duration { get; set; }
        public string? CourseLevel { get; set; }
        public string? CategoryName { get; set; }
        public List<string>? CourseObjective { get; set; } = new();
        public decimal Rating { get; set; }
        public int StudentsCount { get; set; }
    }
}
