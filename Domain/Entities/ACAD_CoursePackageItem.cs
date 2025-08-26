using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.EntityBase;
using Microsoft.EntityFrameworkCore;


namespace Domain.Entities;

public partial class ACAD_CoursePackageItem : IEntityBase
{
    [Key]
    [Column("PackageItemID")]
    public Guid Id { get; set; }

    public Guid PackageID { get; set; }

    public Guid CourseID { get; set; }

    public int Sequence { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ACAD_Course Course { get; set; } = null!;

    public virtual ACAD_CoursePackage Package { get; set; } = null!;
}
