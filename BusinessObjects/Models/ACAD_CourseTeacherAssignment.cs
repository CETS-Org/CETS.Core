using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;

namespace BusinessObjects.Models;

public partial class ACAD_CourseTeacherAssignment : IEntityBase
{
    [Key]
    [Column("AssignmentID")]
    public Guid Id { get; set; }

    public Guid CourseID { get; set; }

    public Guid TeacherID { get; set; }

    [Precision(0)]
    public DateTime AssignedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ACAD_ClassMeeting> ACAD_ClassMeetings { get; set; } = new List<ACAD_ClassMeeting>();

    public virtual ICollection<ACAD_Class> ACAD_Classes { get; set; } = new List<ACAD_Class>();

    public virtual ACAD_Course Course { get; set; } = null!;

    [ForeignKey(nameof(CreatedBy))]
    public virtual IDN_Account? CreatedByNavigation { get; set; }

    public virtual IDN_Teacher Teacher { get; set; } = null!;

    [ForeignKey(nameof(UpdatedBy))]
    public virtual IDN_Account? UpdatedByNavigation { get; set; }
}
