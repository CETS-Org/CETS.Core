using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.EntityBase;
using Microsoft.EntityFrameworkCore;


namespace Domain.Entities;

public partial class FIN_InvoiceItem : IEntityBase
{
    [Key]
    [Column("InvoiceItemID")]
    public Guid Id { get; set; }

    public Guid InvoiceID { get; set; }

    public Guid? CourseID { get; set; }

    public Guid? CoursePackageID { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    public decimal Subtotal { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    public decimal Total { get; set; }

    public Guid? PromotionID { get; set; }

    [ForeignKey(nameof(CourseID))]
    public virtual ACAD_Course? Course { get; set; }

    [ForeignKey(nameof(CoursePackageID))]
    public virtual ACAD_CoursePackage? CoursePackage { get; set; }

    [ForeignKey(nameof(InvoiceID))]
    public virtual FIN_Invoice Invoice { get; set; } = null!;

    [ForeignKey(nameof(PromotionID))]
    public virtual FIN_Promotion? Promotion { get; set; }
}
