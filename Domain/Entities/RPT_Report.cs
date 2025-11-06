using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Domain.Entities.EntityBases.AuditableInterfaces;


namespace Domain.Entities;

public partial class RPT_Report : EntityBase, IHasCreationTime
{
    public Guid ReportTypeID { get; set; }

    public Guid SubmittedBy { get; set; }

    [StringLength(255)]
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? AttachmentUrl { get; set; }

    public Guid ReportStatusID { get; set; }

    public string? ReportUrl { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? ResolvedAt { get; set; }

    public Guid? ResolvedBy { get; set; }

    [ForeignKey(nameof(ReportStatusID))]
    public virtual CORE_LookUp ReportStatus { get; set; } = null!;

    [ForeignKey(nameof(ReportTypeID))]
    public virtual CORE_LookUp ReportType { get; set; } = null!;

    [ForeignKey(nameof(SubmittedBy))]
    public virtual IDN_Account SubmittedByNavigation { get; set; } = null!;

    [ForeignKey(nameof(ResolvedBy))]
    public virtual IDN_Account? ResolvedByNavigation { get; set; }
}
