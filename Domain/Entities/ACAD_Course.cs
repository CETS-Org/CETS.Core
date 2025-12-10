using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;


namespace Domain.Entities;
public partial class ACAD_Course : AuditedEntity
{
    [StringLength(50)]
    [Unicode(false)]
    public string CourseCode { get; set; } = null!;

    [StringLength(255)]
    public string CourseName { get; set; } = null!;

    public Guid CourseLevelID { get; set; }

    public Guid CourseFormatID { get; set; }

    public string? CourseImageUrl { get; set; }

    public List<string>? CourseObjective { get; set; } = new();

    public Guid CategoryID { get; set; }

    public string? Description { get; set; }

    public decimal StandardPrice { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public decimal? AverageRating { get; set; }

    public decimal StandardScore { get; set; }

    public decimal ExitScore { get; set; }

    public virtual ICollection<ACAD_CoursePackageItem> ACAD_CoursePackageItems { get; set; } = new List<ACAD_CoursePackageItem>();

    public virtual ICollection<ACAD_CourseTeacherAssignment> ACAD_CourseTeacherAssignments { get; set; } = new List<ACAD_CourseTeacherAssignment>();

    public virtual ICollection<ACAD_Enrollment> ACAD_Enrollments { get; set; } = new List<ACAD_Enrollment>();

    public virtual ICollection<ACAD_Syllabus> ACAD_Syllabi { get; set; } = new List<ACAD_Syllabus>();

    public virtual ICollection<COM_Feedback> COM_Feedbacks { get; set; } = new List<COM_Feedback>();

    [ForeignKey(nameof(CategoryID))]
    public virtual ACAD_CourseCategory Category { get; set; } = null!;

    [ForeignKey(nameof(CourseFormatID))]
    public virtual CORE_LookUp CourseFormat { get; set; } = null!;

    [ForeignKey(nameof(CourseLevelID))]
    public virtual CORE_LookUp CourseLevel { get; set; } = null!;

    [ForeignKey(nameof(CreatedBy))]
    public virtual IDN_Account? CreatedByNavigation { get; set; }

    public virtual ICollection<FIN_InvoiceItem> FIN_InvoiceItems { get; set; } = new List<FIN_InvoiceItem>();

    public virtual ICollection<ACAD_ReservationItem> ACAD_ReservationItems { get; set; } = new List<ACAD_ReservationItem>();

    [ForeignKey(nameof(UpdatedBy))]
    public virtual IDN_Account? UpdatedByNavigation { get; set; }

    public virtual ICollection<ACAD_CourseBenefit> ACAD_CourseBenefits { get; set; } = new List<ACAD_CourseBenefit>();
    public virtual ICollection<ACAD_CourseRequirement> ACAD_CourseRequirements { get; set; } = new List<ACAD_CourseRequirement>();
    public virtual ICollection<ACAD_CourseSkill> ACAD_CourseSkills { get; set; } = new List<ACAD_CourseSkill>();
    public virtual ICollection<ACAD_CourseSchedule> ACAD_CourseSchedules { get; set; } = new List<ACAD_CourseSchedule>();
    public virtual ICollection<ACAD_CourseWishlist> ACAD_CourseWishlists { get; set; } = new List<ACAD_CourseWishlist>();
}
