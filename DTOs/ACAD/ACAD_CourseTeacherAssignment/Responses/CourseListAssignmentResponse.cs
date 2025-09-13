using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_CourseTeacherAssignment.Responses
{
    public class CourseListAssignmentResponse
    {
        public Guid CourseId { get; set; }
        public string CourseCode { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public string? Description { get; set; }
        public string? CourseImageUrl { get; set; }

        public string CategoryName { get; set; } = null!;
        public string CourseLevelName { get; set; } = null!;
        public string CourseFormatName { get; set; } = null!;

        public int StudentCount { get; set; }
        public DateTime AssignedAt { get; set; }
    }
}
