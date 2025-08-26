using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class FIN_InvoiceItem
{
    [Key]
    public Guid InvoiceItemID { get; set; }

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

    public virtual ACAD_Course? Course { get; set; }

    public virtual ACAD_CoursePackage? CoursePackage { get; set; }

    public virtual FIN_Invoice Invoice { get; set; } = null!;

    public virtual FIN_Promotion? Promotion { get; set; }
}
