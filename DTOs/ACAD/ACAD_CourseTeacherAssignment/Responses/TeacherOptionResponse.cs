using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_CourseTeacherAssignment.Responses
{
    public class TeacherOptionResponse
    {
        public Guid Id { get; set; }           
        public string? FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? AvatarUrl { get; set; }
     
       
        public int? YearsExperience { get; set; }
        public bool CanTeachOnline { get; set; }
        public bool CanTeachOffline { get; set; }
    }
}
