using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.EntityBase;
using Microsoft.EntityFrameworkCore;


namespace Domain.Entities;

public partial class IDN_Teacher : IEntityBase
{
    [Key]
    [Column("AccountID")]
    public Guid Id { get; set; }

    [StringLength(20)]
    public string TeacherCode { get; set; } = null!;

    public int? YearsExperience { get; set; }

    public string? Bio { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<ACAD_Assignment> ACAD_AssignmentCreatedByNavigations { get; set; } = new List<ACAD_Assignment>();

    public virtual ICollection<ACAD_Assignment> ACAD_AssignmentUpdatedByNavigations { get; set; } = new List<ACAD_Assignment>();

    public virtual ICollection<ACAD_Attendance> ACAD_Attendances { get; set; } = new List<ACAD_Attendance>();

    public virtual ICollection<ACAD_CourseTeacherAssignment> ACAD_CourseTeacherAssignments { get; set; } = new List<ACAD_CourseTeacherAssignment>();

    public virtual IDN_Account Account { get; set; } = null!;


    public virtual ICollection<COM_Feedback> COM_Feedbacks { get; set; } = new List<COM_Feedback>();


    public virtual ICollection<HR_Contract> HR_Contracts { get; set; } = new List<HR_Contract>();


    public virtual ICollection<HR_TeacherAvailability> HR_TeacherAvailabilities { get; set; } = new List<HR_TeacherAvailability>();


    public virtual ICollection<IDN_TeacherCredential> IDN_TeacherCredentials { get; set; } = new List<IDN_TeacherCredential>();

    [ForeignKey(nameof(UpdatedBy))]
    public virtual IDN_Account? UpdatedByNavigation { get; set; }
}
