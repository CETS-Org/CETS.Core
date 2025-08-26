using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;
public partial class ACAD_Attendance
{
    [Key]
    public Guid AttendanceID { get; set; }

    public Guid MeetingID { get; set; }

    public Guid StudentID { get; set; }

    public Guid AttendanceStatusID { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public Guid? CheckBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    public virtual CORE_LookUp AttendanceStatus { get; set; } = null!;
    public virtual IDN_Teacher? CheckByNavigation { get; set; }

    public virtual ACAD_ClassMeeting Meeting { get; set; } = null!;
    public virtual IDN_Student Student { get; set; } = null!;
    public virtual IDN_Account? UpdatedByNavigation { get; set; }
}
