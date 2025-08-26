using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class ACAD_CoursePackageItem
{
    [Key]
    public Guid PackageItemID { get; set; }

    public Guid PackageID { get; set; }

    public Guid CourseID { get; set; }

    public int Sequence { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ACAD_Course Course { get; set; } = null!;

    public virtual ACAD_CoursePackage Package { get; set; } = null!;
}
