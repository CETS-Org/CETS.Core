using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.EntityBase;
using Microsoft.EntityFrameworkCore;


namespace Domain.Entities;

public partial class COM_Feedback : IEntityBase
{
    [Key]
    [Column("FeedbackID")]
    public Guid Id { get; set; }

    public Guid SubmitterID { get; set; }

    public Guid? FeedbackTypeID { get; set; }

    public Guid? CourseID { get; set; }

    public Guid? TeacherID { get; set; }

    public int? Rating { get; set; }

    public string Comment { get; set; } = null!;

    [Precision(0)]
    public DateTime SubmittedAt { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey(nameof(CourseID))]
    public virtual ACAD_Course? Course { get; set; }

    [ForeignKey(nameof(FeedbackTypeID))]
    public virtual CORE_LookUp? FeedbackType { get; set; }

    [ForeignKey(nameof(SubmitterID))]
    public virtual IDN_Student Submitter { get; set; } = null!;

    [ForeignKey(nameof(TeacherID))]
    public virtual IDN_Teacher? Teacher { get; set; }
}
