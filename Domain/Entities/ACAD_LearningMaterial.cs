using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities;

public partial class ACAD_LearningMaterial : EntityBase
{
    public Guid UploaderID { get; set; }

    public Guid? ClassID { get; set; }

    [StringLength(255)]
    public string Title { get; set; } = null!;

    public string? StoreUrl { get; set; }

    [Precision(0)]
    public DateTime UploadDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey(nameof(ClassID))]
    public virtual ACAD_Class? Class { get; set; }

    [ForeignKey(nameof(CreatedBy))]
    public virtual IDN_Account? CreatedByNavigation { get; set; }

    [ForeignKey(nameof(UpdatedBy))]
    public virtual IDN_Account? UpdatedByNavigation { get; set; }

    [ForeignKey(nameof(UploaderID))]
    public virtual IDN_Account Uploader { get; set; } = null!;
}
