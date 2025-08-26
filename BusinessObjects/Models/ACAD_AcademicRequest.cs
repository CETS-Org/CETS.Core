using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class ACAD_AcademicRequest
{
    [Key]
    public Guid RequestID { get; set; }

    public Guid StudentID { get; set; }

    public Guid RequestTypeID { get; set; }

    public Guid AcademicRequestStatusID { get; set; }

    public string Reason { get; set; } = null!;

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    public Guid? FromClassID { get; set; }

    public Guid? ToClassID { get; set; }

    public DateOnly? EffectiveDate { get; set; }

    public string? AttachmentUrl { get; set; }

    public Guid? ProcessedBy { get; set; }

    [Precision(0)]
    public DateTime? ProcessedAt { get; set; }

    public virtual ICollection<ACAD_AcademicRequestHistory> ACAD_AcademicRequestHistories { get; set; } = new List<ACAD_AcademicRequestHistory>();

    public virtual CORE_LookUp AcademicRequestStatus { get; set; } = null!;

    public virtual ACAD_Class? FromClass { get; set; }

    public virtual IDN_Account? ProcessedByNavigation { get; set; }

    public virtual CORE_LookUp RequestType { get; set; } = null!;

    public virtual IDN_Student Student { get; set; } = null!;

    public virtual ACAD_Class? ToClass { get; set; }
}
