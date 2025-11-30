using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_AcademicRequest.Responses
{
    public class AcademicRequestResponse
    {
        public Guid Id { get; set; }
        public Guid StudentID { get; set; }
        public string? StudentName { get; set; }
        public string? StudentEmail { get; set; }

        public Guid RequestTypeID { get; set; }
        public string? RequestTypeName { get; set; }

        public Guid AcademicRequestStatusID { get; set; }
        public string? StatusName { get; set; }

        public Guid? PriorityID { get; set; }
        public string? PriorityName { get; set; }

        public string Reason { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public Guid? FromClassID { get; set; }
        public string? FromClassName { get; set; }

        public Guid? ToClassID { get; set; }
        public string? ToClassName { get; set; }

        public DateOnly? EffectiveDate { get; set; }

        // For class transfer - specific meeting details
        public DateOnly? FromMeetingDate { get; set; }
        public Guid? FromSlotID { get; set; }
        public string? FromSlotName { get; set; }
        public DateOnly? ToMeetingDate { get; set; }
        public Guid? ToSlotID { get; set; }
        public string? ToSlotName { get; set; }
        public string? AttachmentUrl { get; set; }

        public Guid? ProcessedBy { get; set; }
        public string? ProcessedByName { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string? StaffResponse { get; set; }

        // For meeting reschedule requests
        public Guid? ClassMeetingID { get; set; }
        public string? MeetingInfo { get; set; } // e.g., "2024-12-15 - Slot1"
        // New meeting details
        public Guid? NewRoomID { get; set; }
        public string? NewRoomName { get; set; }

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
    }
}
