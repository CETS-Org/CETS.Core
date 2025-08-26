using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;

namespace BusinessObjects.Models;
public partial class ACAD_ClassMeeting : IEntityBase
{
    [Key]
    [Column("MeetingID")]
    public Guid Id { get; set; }

    public Guid ClassID { get; set; }

    [Precision(0)]
    public DateTime StartsAt { get; set; }

    [Precision(0)]
    public DateTime EndsAt { get; set; }

    public Guid? RoomID { get; set; }

    public Guid? TeacherAssignmentID { get; set; }

    public string? OnlineMeetingUrl { get; set; }

    [StringLength(100)]
    public string? Passcode { get; set; }

    public string? RecordingUrl { get; set; }

    public string? ProgressNote { get; set; }

    public Guid CoveredTopicID { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<ACAD_Assignment> ACAD_Assignments { get; set; } = new List<ACAD_Assignment>();

    public virtual ICollection<ACAD_Attendance> ACAD_Attendances { get; set; } = new List<ACAD_Attendance>();

    public virtual ACAD_Class Class { get; set; } = null!;

    public virtual ACAD_SyllabusItem CoveredTopic { get; set; } = null!;

    [ForeignKey(nameof(CreatedBy))]
    public virtual IDN_Account? CreatedByNavigation { get; set; }

    public virtual FAC_Room? Room { get; set; }

    public virtual ACAD_CourseTeacherAssignment? TeacherAssignment { get; set; }

    [ForeignKey(nameof(UpdatedBy))]
    public virtual IDN_Account? UpdatedByNavigation { get; set; }
}
