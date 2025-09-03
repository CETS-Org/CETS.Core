using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities;

public partial class FIN_Invoice : AuditedEntity
{
    public Guid StudentID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string InvoiceNumber { get; set; } = null!;

    public Guid InvoiceStatusID { get; set; }

    public int? InvoiceSequence { get; set; }

    public DateOnly CreateDate { get; set; }

    public DateOnly? DueDate { get; set; }

    public decimal Subtotal { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal TotalAmount { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? SeriesID { get; set; }

    public int? PaymentSequence { get; set; }

    public Guid? PlanTypeID { get; set; }

    public virtual ICollection<ACAD_ClassReservation> ACAD_ClassReservations { get; set; } = new List<ACAD_ClassReservation>();

    public virtual ICollection<FIN_InvoiceItem> FIN_InvoiceItems { get; set; } = new List<FIN_InvoiceItem>();

    public virtual ICollection<FIN_Payment> FIN_Payments { get; set; } = new List<FIN_Payment>();

    [ForeignKey(nameof(InvoiceStatusID))]
    public virtual CORE_LookUp InvoiceStatus { get; set; } = null!;

    [ForeignKey(nameof(PlanTypeID))]
    public virtual CORE_LookUp? PlanType { get; set; }

    [ForeignKey(nameof(StudentID))]
    public virtual IDN_Student Student { get; set; } = null!;

    [ForeignKey(nameof(CreatedBy))]
    public virtual IDN_Account? CreatedByNavigation { get; set; }

    [ForeignKey(nameof(UpdatedBy))]
    public virtual IDN_Account? UpdatedByNavigation { get; set; }
}
