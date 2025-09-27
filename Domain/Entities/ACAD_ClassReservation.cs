using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;


namespace Domain.Entities;

public partial class ACAD_ClassReservation : EntityBase
{
    public Guid StudentID { get; set; }

    public Guid? CoursePackageID { get; set; }

    [Precision(0)]
    public DateTime ExpiresAt { get; set; }

    [ForeignKey(nameof(StudentID))]
    public virtual IDN_Student Student { get; set; } = null!;

    [ForeignKey(nameof(CoursePackageID))]
    public virtual ACAD_CoursePackage? CoursePackage { get; set; }

}
