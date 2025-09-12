using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.IDN.IDN_Student.Responses
{
    public class StudentProfileResponse
    {
        // Account info
        public Guid AccountID { get; set; }
        public string FullName { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CID { get; set; }
        public string? Address { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? AvatarUrl { get; set; }

        // Student info
        public string? GuardianName { get; set; }
        public string? GuardianPhone { get; set; }
        public string? School { get; set; }
        public string? AcademicNote { get; set; }
    }
}
