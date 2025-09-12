using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.IDN.IDN_Account.Requests
{
    public class AccountFilterRequest
    {
        public string? RoleName { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public string? SortOrder { get; set; }
        public string? SortBy { get; set; }
        public string? StatusName { get; set; }
        // mô phỏng phân quyền (Admin, Staff, Student, Teacher)
        public string? CurrentRole { get; set; }

    }
}
