using DTOs.ACAD.ACAD_Course.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Class.Responses
{
    public class ClassStaffViewResponse
    {
        public Guid Id { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public string ClassStatus { get; set; } = string.Empty;
        public string ClassFormat { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int Capacity { get; set; }
        public int EnrolledCount { get; set; }
        public bool IsActive { get; set; }

    }
}
