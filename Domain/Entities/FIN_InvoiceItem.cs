using Domain.Entities.EntityBases;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities;

public partial class FIN_InvoiceItem : EntityBase
{
    public Guid InvoiceID { get; set; }

    public Guid? CourseID { get; set; }

    public Guid? CoursePackageID { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Total { get; set; }
    public DateOnly? DueDate { get; set; }

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
