using Domain.Entities.EntityBases;
using System.ComponentModel.DataAnnotations;


namespace Domain.Entities;


public partial class IDN_Role : EntityBase
{
    [StringLength(50)]
    public string RoleName { get; set; } = null!;

    public virtual ICollection<IDN_AccountRole> IDN_AccountRoles { get; set; } = new List<IDN_AccountRole>();
}
