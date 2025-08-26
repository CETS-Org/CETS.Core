using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.EntityBase;
using Microsoft.EntityFrameworkCore;


namespace Domain.Entities;

public partial class ACAD_LearningMaterial : IEntityBase
{
    [Key]
    [Column("MaterialID")]
    public Guid Id { get; set; }

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

    public virtual ACAD_Class? Class { get; set; }

    [ForeignKey(nameof(CreatedBy))]
    public virtual IDN_Account? CreatedByNavigation { get; set; }

    [ForeignKey(nameof(UpdatedBy))]
    public virtual IDN_Account? UpdatedByNavigation { get; set; }

    [ForeignKey(nameof(UploaderID))]
    public virtual IDN_Account Uploader { get; set; } = null!;
}
