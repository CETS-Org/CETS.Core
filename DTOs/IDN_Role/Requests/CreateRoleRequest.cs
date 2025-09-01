using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.IDN_Role.Requests
{
    public class CreateRoleRequest
    {
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; } = null!;
    }
}


