using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_CourseTeacherAssignment.Responses
{
    public class TeachingClassResponse
    {
        public Guid TeachingClassId { get; set; }
        public string CourseName { get; set; } = null!;
        public string CourseCode { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string CourseFormatName { get; set; } = null!;
        public string CourseLevelName { get; set; } = null!;
        public ClassSession classSession { get; set; } = null!;
    }
}
