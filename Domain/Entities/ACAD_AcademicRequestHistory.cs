using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using static Domain.Entities.EntityBases.AuditableInterfaces;


namespace Domain.Entities;

public partial class ACAD_AcademicRequestHistory : EntityBase, IHasModificationTime, IHasModifier
{
    public Guid RequestID { get; set; }

    public Guid StatusID { get; set; }

    public string? Description { get; set; }

    public Guid? UpdatedBy { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    public string? AttachmentUrl { get; set; }

    [ForeignKey(nameof(UpdatedBy))]
    public virtual IDN_Account? UpdatedByNavigation { get; set; }

    [ForeignKey(nameof(RequestID))]
    public virtual ACAD_AcademicRequest Request { get; set; } = null!;

    [ForeignKey(nameof(StatusID))]
    public virtual CORE_LookUp Status { get; set; } = null!;
}
