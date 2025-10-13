using DTOs.ACAD.ACAD_Assignment.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Course.Responses
{
    public class StudentCourseDetailResponse
    {
        public Guid CourseId { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public List<string> TeacherNames { get; set; } = new();
        public string StatusCode { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;

        public List<StudentAssignmentResponse> Assignments { get; set; } = new();
    }
}
