using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using static Domain.Entities.EntityBases.AuditableInterfaces;


namespace Domain.Entities;
public partial class ACAD_Attendance : EntityBase, IHasCreationTime, IHasModificationTime, IHasModifier
{

    public Guid MeetingID { get; set; }

    public Guid StudentID { get; set; }

    public Guid AttendanceStatusID { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }
    public Guid? CheckedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey(nameof(AttendanceStatusID))]
    public virtual CORE_LookUp AttendanceStatus { get; set; } = null!;

    [ForeignKey(nameof(CheckedBy))]
    public virtual IDN_Teacher? CheckedByNavigation { get; set; }

    [ForeignKey(nameof(MeetingID))]
    public virtual ACAD_ClassMeeting Meeting { get; set; } = null!;

    [ForeignKey(nameof(StudentID))]
    public virtual IDN_Student Student { get; set; } = null!;
    
    [ForeignKey(nameof(UpdatedBy))]
    public virtual IDN_Account? UpdatedByNavigation { get; set; }
}
