using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace Domain.Entities;

public partial class CORE_LookUpType : EntityBase
{

    [StringLength(50)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(100)]
    public string Name { get; set; } = null!;

    public virtual ICollection<CORE_LookUp> CORE_LookUps { get; set; } = new List<CORE_LookUp>();
}
