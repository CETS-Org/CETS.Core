using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;

namespace BusinessObjects.Models;

public partial class IDN_Student : IEntityBase
{
    [Key]
    [Column("AccountID")]
    public Guid Id { get; set; }

    [StringLength(20)]
    public string StudentCode { get; set; } = null!;

    [StringLength(100)]
    public string? GuardianName { get; set; }

    [StringLength(20)]
    public string? GuardianPhone { get; set; }

    [StringLength(150)]
    public string? School { get; set; }

    public string? AcademicNote { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    
    public virtual ICollection<ACAD_AcademicRequest> ACAD_AcademicRequests { get; set; } = new List<ACAD_AcademicRequest>();

    
    public virtual ICollection<ACAD_Attendance> ACAD_Attendances { get; set; } = new List<ACAD_Attendance>();

    
    public virtual ICollection<ACAD_ClassReservation> ACAD_ClassReservations { get; set; } = new List<ACAD_ClassReservation>();

    
    public virtual ICollection<ACAD_Enrollment> ACAD_Enrollments { get; set; } = new List<ACAD_Enrollment>();

    public virtual ICollection<ACAD_Submission> ACAD_Submissions { get; set; } = new List<ACAD_Submission>();

    public virtual IDN_Account Account { get; set; } = null!;

    public virtual ICollection<COM_Feedback> COM_Feedbacks { get; set; } = new List<COM_Feedback>();

    public virtual ICollection<FIN_Invoice> FIN_Invoices { get; set; } = new List<FIN_Invoice>();

    [ForeignKey(nameof(UpdatedBy))]
    public virtual IDN_Account? UpdatedByNavigation { get; set; }
}
