using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;

namespace BusinessObjects.Models;

public partial class ACAD_Submission : IEntityBase
{
    [Key]
    [Column("SubmissionID")]
    public Guid Id { get; set; }

    public Guid StudentID { get; set; }

    public Guid? AssignmentID { get; set; }

    [StringLength(255)]
    public string? Title { get; set; }

    public string? StoreUrl { get; set; }

    [Precision(0)]
    public DateTime SubmittedAt { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? Score { get; set; }

    public string? Feedback { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ACAD_Assignment? Assignment { get; set; }

    [ForeignKey(nameof(CreatedBy))]
    public virtual IDN_Account? CreatedByNavigation { get; set; }

    public virtual IDN_Student Student { get; set; } = null!;

    [ForeignKey(nameof(UpdatedBy))]
    public virtual IDN_Account? UpdatedByNavigation { get; set; }
}
