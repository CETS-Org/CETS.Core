using Domain.Entities.EntityBases;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities;

public partial class IDN_AccountRole : EntityBase
{
    public Guid AccountID { get; set; }

    public Guid RoleID { get; set; }

    [ForeignKey(nameof(AccountID))]
    public virtual IDN_Account Account { get; set; } = null!;

    [ForeignKey(nameof(RoleID))]
    public virtual IDN_Role Role { get; set; } = null!;
}
