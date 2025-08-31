using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.IDN_Account.Requests
{
    public class UpdateAccountRequest
    {
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

        public bool IsDeleted { get; set; }
    }
}
