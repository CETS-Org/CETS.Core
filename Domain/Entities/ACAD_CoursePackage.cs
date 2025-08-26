using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.EntityBase;
using Microsoft.EntityFrameworkCore;


namespace Domain.Entities;

public partial class ACAD_CoursePackage : IEntityBase
{
    [Key]
    [Column("PackageID")]
    public Guid Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string PackageCode { get; set; } = null!;

    [StringLength(255)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal TotalPrice { get; set; }

    public bool IsActive { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<ACAD_CoursePackageItem> ACAD_CoursePackageItems { get; set; } = new List<ACAD_CoursePackageItem>();

    public virtual IDN_Account? CreatedByNavigation { get; set; }

    public virtual ICollection<FIN_InvoiceItem> FIN_InvoiceItems { get; set; } = new List<FIN_InvoiceItem>();

    public virtual IDN_Account? UpdatedByNavigation { get; set; }
}
