using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.IDN.IDN_Account.Requests
{
    public class CreateAccountRequest
    {
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string FullName { get; set; } = null!;
        public DateOnly? DateOfBirth { get; set; }
        public string? CID { get; set; }         
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }

        public Guid RoleID { get; set; } 


    }
}
