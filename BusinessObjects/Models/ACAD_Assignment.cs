using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class ACAD_Assignment
{
    [Key]
    public Guid AssignmentID { get; set; }

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

    public virtual ACAD_ClassMeeting? ClassMeeting { get; set; }

    public virtual IDN_Teacher CreatedByNavigation { get; set; } = null!;

    public virtual IDN_Teacher? UpdatedByNavigation { get; set; }
}
