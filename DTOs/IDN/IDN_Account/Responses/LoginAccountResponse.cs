using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.IDN.IDN_Authentication;
using DTOs.IDN.IDN_Staff;
using DTOs.IDN.IDN_Student.Responses;
using DTOs.IDN.IDN_Teacher.Responses;

namespace DTOs.IDN.IDN_Account.Responses
{
    public class LoginAccountResponse : IAppUser
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public List<string> RoleNames { get; set; } = new();
        public bool IsVerified { get; set; }
        public string? VerifiedCode { get; set; }
        public DateTime? VerifiedCodeExpiresAt { get; set; }
        public StudentResponse? StudentInfo { get; set; }
        public TeacherDetailResponse? TeacherInfo { get; set; }
        public StaffDetailResponse? StaffInfo { get; set; }
    }
}
