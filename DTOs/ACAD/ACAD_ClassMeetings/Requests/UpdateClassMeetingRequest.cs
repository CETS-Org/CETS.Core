using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_ClassMeetings.Requests
{
    public class UpdateClassMeetingRequest
    {
        public Guid Id { get; set; }

        public Guid SlotID { get; set; }

        public DateOnly Date { get; set; }

        public bool IsStudy { get; set; }

        public Guid? RoomID { get; set; }

        public Guid? TeacherAssignmentID { get; set; }

        public string? OnlineMeetingUrl { get; set; }

        [StringLength(100)]
        public string? Passcode { get; set; }

        public string? RecordingUrl { get; set; }

        public string? ProgressNote { get; set; }

        public Guid CoveredTopicID { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }
}
