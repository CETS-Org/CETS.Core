using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;

namespace BusinessObjects.Models;

public partial class ACAD_Class : IEntityBase
{
    [Key]
    [Column("ClassID")]
    public Guid Id { get; set; }

    public Guid ClassStatusID { get; set; }

    public Guid? CourseFormatID { get; set; }

    public Guid? TeacherAssignmentID { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int Capacity { get; set; }

    public int EnrolledCount { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<ACAD_AcademicRequest> ACAD_AcademicRequestFromClasses { get; set; } = new List<ACAD_AcademicRequest>();

    public virtual ICollection<ACAD_AcademicRequest> ACAD_AcademicRequestToClasses { get; set; } = new List<ACAD_AcademicRequest>();

    public virtual ICollection<ACAD_ClassMeeting> ACAD_ClassMeetings { get; set; } = new List<ACAD_ClassMeeting>();

    public virtual ICollection<ACAD_ClassReservation> ACAD_ClassReservations { get; set; } = new List<ACAD_ClassReservation>();

    public virtual ICollection<ACAD_Enrollment> ACAD_Enrollments { get; set; } = new List<ACAD_Enrollment>();

    public virtual ICollection<ACAD_LearningMaterial> ACAD_LearningMaterials { get; set; } = new List<ACAD_LearningMaterial>();

    public virtual CORE_LookUp ClassStatus { get; set; } = null!;

    public virtual CORE_LookUp? CourseFormat { get; set; }

    [ForeignKey(nameof(CreatedBy))]
    public virtual IDN_Account? CreatedByNavigation { get; set; }

    public virtual ACAD_CourseTeacherAssignment? TeacherAssignment { get; set; }

    [ForeignKey(nameof(UpdatedBy))]
    public virtual IDN_Account? UpdatedByNavigation { get; set; }
}
