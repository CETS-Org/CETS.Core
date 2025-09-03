using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.IDN.IDN_Role.Requests
{
    public class UpdateRoleRequest
    {
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; } = null!;
    }
}


