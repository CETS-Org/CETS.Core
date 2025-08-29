using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;


namespace Domain.Entities;

public partial class ACAD_CourseCategory : EntityBase
{
    [StringLength(50)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(200)]
    public string Name { get; set; } = null!;

    public virtual ICollection<ACAD_Course> ACAD_Courses { get; set; } = new List<ACAD_Course>();
}
