using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.COM.COM_Chat.Responses
{
    public class UserBasicInfo
    {
        public Guid Id { get; set; } // IDN_Account dùng Guid
        public string FullName { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
    }
}
