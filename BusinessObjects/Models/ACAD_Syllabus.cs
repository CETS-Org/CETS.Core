using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class ACAD_Syllabus
{
    [Key]
    public Guid SyllabusID { get; set; }

    public Guid CourseID { get; set; }

    [StringLength(255)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<ACAD_SyllabusItem> ACAD_SyllabusItems { get; set; } = new List<ACAD_SyllabusItem>();

    public virtual ACAD_Course Course { get; set; } = null!;

    public virtual IDN_Account? CreatedByNavigation { get; set; }

    public virtual IDN_Account? UpdatedByNavigation { get; set; }
}
