using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities;

public partial class ACAD_Submission : EntityBase
{
    public Guid? AssignmentID { get; set; }

    public Guid StudentID { get; set; }

    public string? StoreUrl { get; set; }

    public string? Feedback { get; set; }

    public decimal? Score { get; set; }

    [Precision(0)]
    public DateTime SubmittedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey(nameof(AssignmentID))]
    public virtual ACAD_Assignment? Assignment { get; set; }

    [ForeignKey(nameof(CreatedBy))]
    public virtual IDN_Account? CreatedByNavigation { get; set; }

    [ForeignKey(nameof(StudentID))]
    public virtual IDN_Student Student { get; set; } = null!;

    [ForeignKey(nameof(UpdatedBy))]
    public virtual IDN_Account? UpdatedByNavigation { get; set; }
}
