using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities;

public partial class ACAD_CoursePackage : AuditedEntity
{
    [StringLength(50)]
    [Unicode(false)]
    public string PackageCode { get; set; } = null!;

    [StringLength(255)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal TotalPrice { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<ACAD_CoursePackageItem> ACAD_CoursePackageItems { get; set; } = new List<ACAD_CoursePackageItem>();

    [ForeignKey(nameof(CreatedBy))]
    public virtual IDN_Account? CreatedByNavigation { get; set; }

    public virtual ICollection<FIN_InvoiceItem> FIN_InvoiceItems { get; set; } = new List<FIN_InvoiceItem>();

    [ForeignKey(nameof(UpdatedBy))]
    public virtual IDN_Account? UpdatedByNavigation { get; set; }
}
