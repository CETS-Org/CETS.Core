using System;

namespace DTOs.IDN_AccountRole.Responses
{
    public class AccountRoleResponse
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}


