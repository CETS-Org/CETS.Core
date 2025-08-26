using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class IDN_AccountRole
{
    [Key]
    public Guid AccountRoleID { get; set; }

    public Guid AccountID { get; set; }

    public Guid RoleID { get; set; }

    public virtual IDN_Account Account { get; set; } = null!;

    public virtual IDN_Role Role { get; set; } = null!;
}
