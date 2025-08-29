using Domain.Entities.EntityBases;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities;

public partial class ACAD_CoursePackageItem : EntityBase
{
    public Guid PackageID { get; set; }

    public Guid CourseID { get; set; }

    public int Sequence { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey(nameof(CourseID))]
    public virtual ACAD_Course Course { get; set; } = null!;

    [ForeignKey(nameof(PackageID))]
    public virtual ACAD_CoursePackage Package { get; set; } = null!;
}
