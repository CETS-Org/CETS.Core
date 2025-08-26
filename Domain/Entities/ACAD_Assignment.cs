using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.EntityBase;
using Microsoft.EntityFrameworkCore;


namespace Domain.Entities;

public partial class ACAD_Assignment : IEntityBase
{
    [Key]
    [Column("AssignmentID")]
    public Guid Id { get; set; }

    public Guid? ClassMeetingID { get; set; }

    [StringLength(255)]
    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? StoreUrl { get; set; }

    [Precision(0)]
    public DateTime? DueAt { get; set; }

    public Guid CreatedBy { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<ACAD_Submission> ACAD_Submissions { get; set; } = new List<ACAD_Submission>();

    [ForeignKey(nameof(ClassMeetingID))]
    public virtual ACAD_ClassMeeting? ClassMeeting { get; set; }

    [ForeignKey(nameof(CreatedBy))]
    public virtual IDN_Teacher CreatedByNavigation { get; set; } = null!;

    [ForeignKey(nameof(UpdatedBy))]
    public virtual IDN_Teacher? UpdatedByNavigation { get; set; }
}
