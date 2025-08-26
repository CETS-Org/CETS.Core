using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class RPT_Report
{
    [Key]
    public Guid ReportID { get; set; }

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

    public virtual CORE_LookUp ReportStatus { get; set; } = null!;

    public virtual CORE_LookUp ReportType { get; set; } = null!;

    public virtual IDN_Account? ResolvedByNavigation { get; set; }

    public virtual IDN_Account SubmittedByNavigation { get; set; } = null!;
}
