using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.IDN.IDN_Teacher.Responses
{
    public class TeacherOptionDto
    {
        /// <summary>
        /// Id của ACAD_CourseTeacherAssignment (TeacherAssignmentID)
        /// </summary>
        public Guid Id { get; set; }

        public string FullName { get; set; } = null!;
        public string? Email { get; set; }
    }
}
