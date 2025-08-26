using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.EntityBase;
using Microsoft.EntityFrameworkCore;


namespace Domain.Entities;

public partial class CORE_LookUpType : IEntityBase
{
    [Key]
    [Column("LookUpTypeID")]
    public Guid Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(100)]
    public string Name { get; set; } = null!;

    public virtual ICollection<CORE_LookUp> CORE_LookUps { get; set; } = new List<CORE_LookUp>();
}
