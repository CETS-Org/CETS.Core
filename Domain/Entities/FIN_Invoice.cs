using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.EntityBase;
using Microsoft.EntityFrameworkCore;


namespace Domain.Entities;

public partial class FIN_Invoice : IEntityBase
{
    [Key]
    [Column("InvoiceID")]
    public Guid Id { get; set; }

    public Guid StudentID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string InvoiceNumber { get; set; } = null!;

    public Guid InvoiceStatusID { get; set; }

    public DateOnly CreateDate { get; set; }

    public DateOnly? DueDate { get; set; }

    [Column(TypeName = "decimal(14, 2)")]
    public decimal Subtotal { get; set; }

    [Column(TypeName = "decimal(14, 2)")]
    public decimal TaxAmount { get; set; }

    [Column(TypeName = "decimal(14, 2)")]
    public decimal TotalAmount { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? SeriesID { get; set; }

    public int? Sequence { get; set; }

    public Guid? PlanTypeID { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }
    public virtual ICollection<ACAD_ClassReservation> ACAD_ClassReservations { get; set; } = new List<ACAD_ClassReservation>();

    public virtual ICollection<FIN_InvoiceItem> FIN_InvoiceItems { get; set; } = new List<FIN_InvoiceItem>();

    public virtual ICollection<FIN_Payment> FIN_Payments { get; set; } = new List<FIN_Payment>();

    public virtual CORE_LookUp InvoiceStatus { get; set; } = null!;

    public virtual CORE_LookUp? PlanType { get; set; }

    public virtual IDN_Student Student { get; set; } = null!;

    [ForeignKey(nameof(CreatedBy))]
    public virtual IDN_Account? CreatedByNavigation { get; set; }

    [ForeignKey(nameof(UpdatedBy))]
    public virtual IDN_Account? UpdatedByNavigation { get; set; }
}
