using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_AcademicRequest.Requests
{
    public class CreateAcademicRequest
    {
        public Guid StudentID { get; set; }
        public Guid RequestTypeID { get; set; }
        public Guid? PriorityID { get; set; }
        public string Reason { get; set; } = null!;
        public Guid? FromClassID { get; set; }
        public Guid? ToClassID { get; set; }
        public DateOnly? EffectiveDate { get; set; }
        // For class transfer - specific meeting details
        public DateOnly? FromMeetingDate { get; set; }
        public Guid? FromSlotID { get; set; }
        public DateOnly? ToMeetingDate { get; set; }
        public Guid? ToSlotID { get; set; }
        public string? AttachmentUrl { get; set; }

        // For meeting reschedule requests
        public Guid? ClassMeetingID { get; set; }
        public Guid? NewRoomID { get; set; }

        // For suspension requests
        public DateOnly? SuspensionStartDate { get; set; }
        public DateOnly? SuspensionEndDate { get; set; }
        public string? ReasonCategory { get; set; }

        // For dropout requests
        public bool? CompletedExitSurvey { get; set; }
        public string? ExitSurveyId { get; set; }
    }
}
