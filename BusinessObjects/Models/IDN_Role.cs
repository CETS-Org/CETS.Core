using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;

namespace BusinessObjects.Models;


public partial class IDN_Role : IEntityBase
{
    [Key]
    [Column("RoleID")]
    public Guid Id { get; set; }

    [StringLength(50)]
    public string RoleName { get; set; } = null!;

    public virtual ICollection<IDN_AccountRole> IDN_AccountRoles { get; set; } = new List<IDN_AccountRole>();
}
