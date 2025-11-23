using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_CourseTeacherAssignment.Responses
{
    public class CourseTeacherAssignmentResponse
    {

        public Guid Id { get; set; }
        public string? CourseName { get; set; }
        public string? FullName { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? AvatarUrl { get; set; }
        public int? YearsExperience { get; set; }         
        public DateTime AssignedAt { get; set; }
    }
}
