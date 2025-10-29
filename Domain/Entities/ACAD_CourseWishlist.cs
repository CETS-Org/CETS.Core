using System;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using static Domain.Entities.EntityBases.AuditableInterfaces;

namespace Domain.Entities;

public partial class ACAD_CourseWishlist : EntityBase, IHasCreationTime
{
    public Guid StudentId { get; set; }

    public Guid CourseId { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [ForeignKey(nameof(StudentId))]
    public virtual IDN_Student Student { get; set; } = null!;

    [ForeignKey(nameof(CourseId))]
    public virtual ACAD_Course Course { get; set; } = null!;
}

