using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;


namespace Domain.Entities;
public partial class ACAD_ClassMeeting : AuditedEntity
{
    public Guid ClassID { get; set; }

    public Guid SlotID { get; set; }

    public DateOnly Date { get; set; }

    

    public Guid? RoomID { get; set; }

    public Guid? TeacherAssignmentID { get; set; }

    public string? OnlineMeetingUrl { get; set; }

    [StringLength(100)]
    public string? Passcode { get; set; }

    public string? RecordingUrl { get; set; }

    public string? ProgressNote { get; set; }

    public Guid CoveredTopicID { get; set; }

    public bool IsActive { get; set; } = true;
    public bool IsStudy { get; set; } = true;

    public bool IsDeleted { get; set; }

    public virtual ICollection<ACAD_Assignment> ACAD_Assignments { get; set; } = new List<ACAD_Assignment>();

    public virtual ICollection<ACAD_Attendance> ACAD_Attendances { get; set; } = new List<ACAD_Attendance>();

    public virtual ICollection<ACAD_WeeklyFeedback> ACAD_WeeklyFeedbacks { get; set; } = new List<ACAD_WeeklyFeedback>();

    [ForeignKey(nameof(ClassID))]
    public virtual ACAD_Class Class { get; set; } = null!;

    [ForeignKey(nameof(CoveredTopicID))]
    public virtual ACAD_SyllabusItem CoveredTopic { get; set; } = null!;

    [ForeignKey(nameof(CreatedBy))]
    public virtual IDN_Account? CreatedByNavigation { get; set; }

    [ForeignKey(nameof(RoomID))]
    public virtual FAC_Room? Room { get; set; }

    [ForeignKey(nameof(TeacherAssignmentID))]
    public virtual ACAD_CourseTeacherAssignment? TeacherAssignment { get; set; }

    [ForeignKey(nameof(UpdatedBy))]
    public virtual IDN_Account? UpdatedByNavigation { get; set; }

    [ForeignKey(nameof(SlotID))]
    public virtual CORE_LookUp Slot { get; set; } = null!;
}
