using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;

namespace BusinessObjects.Models;

public partial class ACAD_AcademicRequestHistory : IEntityBase
{
    [Key]
    [Column("HistoryID")]
    public Guid Id { get; set; }

    public Guid RequestID { get; set; }

    public Guid StatusID { get; set; }

    public string? Description { get; set; }

    public Guid? ChangedBy { get; set; }

    [Precision(0)]
    public DateTime ChangedAt { get; set; }

    public string? AttachmentUrl { get; set; }

    [ForeignKey(nameof(ChangedBy))]
    public virtual IDN_Account? ChangedByNavigation { get; set; }

    public virtual ACAD_AcademicRequest Request { get; set; } = null!;

    public virtual CORE_LookUp Status { get; set; } = null!;
}
