using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.IDN.IDN_Account.Requests
{
    public class UpdateAccountProfileRequest
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? CID { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
