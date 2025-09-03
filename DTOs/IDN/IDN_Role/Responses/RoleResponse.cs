using System;

namespace DTOs.IDN.IDN_Role.Responses
{
    public class RoleResponse
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}


