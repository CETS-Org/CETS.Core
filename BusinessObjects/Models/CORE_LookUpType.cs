using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class CORE_LookUpType
{
    [Key]
    public Guid LookUpTypeID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(100)]
    public string Name { get; set; } = null!;

    public virtual ICollection<CORE_LookUp> CORE_LookUps { get; set; } = new List<CORE_LookUp>();
}
