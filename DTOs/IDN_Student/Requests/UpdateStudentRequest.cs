using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.IDN_Student.Requests
{
    public class UpdateStudentRequest
    {
        public string? GuardianName { get; set; }
        public string? GuardianPhone { get; set; }
        public string? School { get; set; }
        public string? AcademicNote { get; set; }
        public bool IsDeleted { get; set; }
    }
}
