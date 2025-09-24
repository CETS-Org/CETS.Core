using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_CourseTeacherAssignment.Responses
{
    public class ClassTeachingListResponse
    {
        public Guid ClassId { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int EnrolledCount { get; set; }
        public bool IsActive { get; set; }
        public string classFormatName { get; set; } = string.Empty;
        public string className { get; set; } = string.Empty;
        public int classNumber { get; set; }
        public ClassSession? classSession { get; set; } 

    }
}
