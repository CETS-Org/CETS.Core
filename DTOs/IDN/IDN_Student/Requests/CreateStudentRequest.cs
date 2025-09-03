using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.IDN.IDN_Student.Requests
{
    public class CreateStudentRequest
    {
        public Guid AccountId { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianPhone { get; set; }
        public string? School { get; set; }
        public string? AcademicNote { get; set; }
    }
}
