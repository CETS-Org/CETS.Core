using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.IDN_AccountRole.Requests
{
    public class UnassignRoleRequest
    {
        [Required]
        public Guid AccountId { get; set; }

        [Required]
        public Guid RoleId { get; set; }
    }
}


