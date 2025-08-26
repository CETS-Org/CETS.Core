using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;
public partial class ACAD_Course
{
    [Key]
    public Guid CourseID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string CourseCode { get; set; } = null!;

    [StringLength(255)]
    public string CourseName { get; set; } = null!;

    public Guid CourseLevelID { get; set; }

    public Guid CourseFormatID { get; set; }

    public Guid CategoryID { get; set; }

    public string? Description { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal StandardPrice { get; set; }

    public bool IsActive { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<ACAD_CoursePackageItem> ACAD_CoursePackageItems { get; set; } = new List<ACAD_CoursePackageItem>();

    public virtual ICollection<ACAD_CourseTeacherAssignment> ACAD_CourseTeacherAssignments { get; set; } = new List<ACAD_CourseTeacherAssignment>();

    public virtual ICollection<ACAD_Enrollment> ACAD_Enrollments { get; set; } = new List<ACAD_Enrollment>();

    public virtual ICollection<ACAD_Syllabus> ACAD_Syllabi { get; set; } = new List<ACAD_Syllabus>();

    public virtual ICollection<COM_Feedback> COM_Feedbacks { get; set; } = new List<COM_Feedback>();

    public virtual ACAD_CourseCategory Category { get; set; } = null!;

    public virtual CORE_LookUp CourseFormat { get; set; } = null!;

    public virtual CORE_LookUp CourseLevel { get; set; } = null!;

    public virtual IDN_Account? CreatedByNavigation { get; set; }

    public virtual ICollection<FIN_InvoiceItem> FIN_InvoiceItems { get; set; } = new List<FIN_InvoiceItem>();

    public virtual IDN_Account? UpdatedByNavigation { get; set; }
}
