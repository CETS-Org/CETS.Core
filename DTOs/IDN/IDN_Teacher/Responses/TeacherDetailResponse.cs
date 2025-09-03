using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.IDN.IDN_Teacher.Responses
{
    public class TeacherDetailResponse
    {
        // Teacher Information
        public Guid TeacherId { get; set; }
        public string TeacherCode { get; set; } = null!;
        public int? YearsExperience { get; set; }
        public string? Bio { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        // Account Information
        public Guid AccountId { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string FullName { get; set; } = null!;
        public DateOnly? DateOfBirth { get; set; }
        public string? CID { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }
        public Guid? AccountStatusID { get; set; }
        public bool IsVerified { get; set; }
        public string? VerifiedCode { get; set; }
        public DateTime? VerifiedCodeExpiresAt { get; set; }
        public DateTime AccountCreatedAt { get; set; }
        public DateTime? AccountUpdatedAt { get; set; }
        public Guid? AccountUpdatedBy { get; set; }
        public bool AccountIsDeleted { get; set; }

        public List<TeacherCredentialDetail> TeacherCredentials { get; set; } = new List<TeacherCredentialDetail>();
    }

    public class TeacherCredentialDetail
    {
        public Guid CredentialId { get; set; }
        public Guid TeacherID { get; set; }
        public Guid CredentialTypeID { get; set; }
        public string? PictureUrl { get; set; }
        public string Name { get; set; } = null!;
        public string Level { get; set; } = null!;
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string? CredentialTypeName { get; set; }
    }
}
