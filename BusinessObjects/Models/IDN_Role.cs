using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;


public partial class IDN_Role
{
    [Key]
    public Guid RoleID { get; set; }

    [StringLength(50)]
    public string RoleName { get; set; } = null!;

    public virtual ICollection<IDN_AccountRole> IDN_AccountRoles { get; set; } = new List<IDN_AccountRole>();
}
