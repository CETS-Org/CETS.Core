using Domain.Entities.EntityBases;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using static Domain.Entities.EntityBases.AuditableInterfaces;

namespace Domain.Entities;

public partial class ACAD_AcademicRequest : EntityBase, IHasCreationTime
{
    public Guid StudentID { get; set; }

    public Guid RequestTypeID { get; set; }

    public Guid AcademicRequestStatusID { get; set; }

    public Guid? PriorityID { get; set; }

    public string Reason { get; set; } = null!;

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    public Guid? FromClassID { get; set; }

    public Guid? ToClassID { get; set; }

    public DateOnly? EffectiveDate { get; set; }

    // For class transfer - specific meeting details
    public DateOnly? FromMeetingDate { get; set; }
    public Guid? FromSlotID { get; set; }
    public DateOnly? ToMeetingDate { get; set; }
    public Guid? ToSlotID { get; set; }

    public string? AttachmentUrl { get; set; }

    public Guid? ProcessedBy { get; set; }

    [Precision(0)]
    public DateTime? ProcessedAt { get; set; }

    public string? StaffResponse { get; set; }

    // For meeting reschedule requests
    public Guid? ClassMeetingID { get; set; }

    public Guid? NewRoomID { get; set; }

    // For suspension requests
    public DateOnly? SuspensionStartDate { get; set; }
    public DateOnly? SuspensionEndDate { get; set; }
    public string? ReasonCategory { get; set; }
    public DateOnly? ExpectedReturnDate { get; set; }

    // For dropout requests
    public bool? CompletedExitSurvey { get; set; }
    public string? ExitSurveyId { get; set; }

    // Related enrollment and payment
    public Guid? EnrollmentID { get; set; }
    public Guid? PaymentID { get; set; }

    public virtual ICollection<ACAD_AcademicRequestHistory> ACAD_AcademicRequestHistories { get; set; } = new List<ACAD_AcademicRequestHistory>();

    [ForeignKey(nameof(AcademicRequestStatusID))]
    public virtual CORE_LookUp AcademicRequestStatus { get; set; } = null!;

    [ForeignKey(nameof(PriorityID))]
    public virtual CORE_LookUp? Priority { get; set; }

    [ForeignKey(nameof(ClassMeetingID))]
    public virtual ACAD_ClassMeeting? ClassMeeting { get; set; }

    [ForeignKey(nameof(FromClassID))]
    public virtual ACAD_Class? FromClass { get; set; }

    [ForeignKey(nameof(NewRoomID))]
    public virtual FAC_Room? NewRoom { get; set; }

    [ForeignKey(nameof(FromSlotID))]
    public virtual CORE_LookUp? FromSlot { get; set; }

    [ForeignKey(nameof(ToSlotID))]
    public virtual CORE_LookUp? ToSlot { get; set; }

    [ForeignKey(nameof(ProcessedBy))]
    public virtual IDN_Account? ProcessedByNavigation { get; set; }

    [ForeignKey(nameof(RequestTypeID))]
    public virtual CORE_LookUp RequestType { get; set; } = null!;

    [ForeignKey(nameof(StudentID))]
    public virtual IDN_Student Student { get; set; } = null!;

    [ForeignKey(nameof(ToClassID))]
    public virtual ACAD_Class? ToClass { get; set; }

    [ForeignKey(nameof(EnrollmentID))]
    public virtual ACAD_Enrollment? Enrollment { get; set; }

    [ForeignKey(nameof(PaymentID))]
    public virtual FIN_Payment? Payment { get; set; }
}
