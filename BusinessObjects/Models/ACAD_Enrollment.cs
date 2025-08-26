using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;

namespace BusinessObjects.Models;

public partial class ACAD_Enrollment : IEntityBase
{
    [Key]
    [Column("EnrollmentID")]
    public Guid Id { get; set; }

    public Guid StudentID { get; set; }

    public Guid? ClassID { get; set; }

    public Guid CourseID { get; set; }

    public Guid EnrollmentStatusID { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey(nameof(ClassID))]
    public virtual ACAD_Class? Class { get; set; }

    [ForeignKey(nameof(CourseID))]
    public virtual ACAD_Course Course { get; set; } = null!;

    [ForeignKey(nameof(CreatedBy))]
    public virtual IDN_Account? CreatedByNavigation { get; set; }

    [ForeignKey(nameof(EnrollmentStatusID))]
    public virtual CORE_LookUp EnrollmentStatus { get; set; } = null!;

    [ForeignKey(nameof(StudentID))]
    public virtual IDN_Student Student { get; set; } = null!;

    [ForeignKey(nameof(UpdatedBy))]
    public virtual IDN_Account? UpdatedByNavigation { get; set; }
}
