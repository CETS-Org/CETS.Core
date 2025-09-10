using DTOs.IDN.IDN_Student.Responses;
using DTOs.IDN.IDN_Teacher.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.IDN.IDN_Account.Responses
{
    public class AccountResponse
    {
        public Guid AccountId { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string FullName { get; set; } = null!;

        public DateOnly? DateOfBirth { get; set; }

        public string? CID { get; set; }

        public string? Address { get; set; }

        public string? AvatarUrl { get; set; }

        public string? Password { get; set; }

        public Guid? AccountStatusID { get; set; }

        public bool IsVerified { get; set; }

        public string? VerifiedCode { get; set; }

        public DateTime? VerifiedCodeExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }

        public string? StatusName { get; set; }
        public List<string>? RoleNames { get; set; }
        public StudentResponse? StudentInfo { get; set; }
        public TeacherDetailResponse? TeacherInfo { get; set; }
    }
}
