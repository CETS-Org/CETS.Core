using DTOs.IDN.IDN_Account.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.IDN.IDN_Student.Requests
{
    public class UpdateStudentProfileRequest
    {
        public string? StudentCode { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianPhone { get; set; }
        public string? School { get; set; }
        public string? AcademicNote { get; set; }

        public string? FullName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? CID { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }
    }

}
