using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_ClassMeetings.Requests
{
    public class CreateClassMeetingRequest
    {
        public Guid ClassID { get; set; }
        public Guid SlotID { get; set; }
        public DateOnly Date { get; set; }
        public Guid? RoomID { get; set; }
        public Guid? TeacherAssignmentID { get; set; }
        public string? OnlineMeetingUrl { get; set; }

        [StringLength(100)]
        public string? Passcode { get; set; }
        public string? ProgressNote { get; set; }
        public Guid CoveredTopicID { get; set; }
    }
}
