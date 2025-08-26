using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.EntityBase;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public partial class ACAD_AcademicRequest : IEntityBase
{
    [Key]
    [Column("RequestID")]
    public Guid Id { get; set; }

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

    [ForeignKey(nameof(AcademicRequestStatusID))]
    public virtual CORE_LookUp AcademicRequestStatus { get; set; } = null!;

    [ForeignKey(nameof(FromClassID))]
    public virtual ACAD_Class? FromClass { get; set; }

    [ForeignKey(nameof(ProcessedBy))]
    public virtual IDN_Account? ProcessedByNavigation { get; set; }

    [ForeignKey(nameof(RequestTypeID))]
    public virtual CORE_LookUp RequestType { get; set; } = null!;

    [ForeignKey(nameof(StudentID))]
    public virtual IDN_Student Student { get; set; } = null!;

    [ForeignKey(nameof(ToClassID))]
    public virtual ACAD_Class? ToClass { get; set; }
}
