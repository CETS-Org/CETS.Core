using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.EntityBase;
using Microsoft.EntityFrameworkCore;


namespace Domain.Entities;

public partial class IDN_AccountRole : IEntityBase
{
    [Key]
    [Column("AccountRoleID")]
    public Guid Id { get; set; }

    public Guid AccountID { get; set; }

    public Guid RoleID { get; set; }

    public virtual IDN_Account Account { get; set; } = null!;

    public virtual IDN_Role Role { get; set; } = null!;
}
