using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_ClassMeetings.Responses
{
    public class ClassMeetingStaffViewResponse
    {
        public Guid Id { get; set; }
        public Guid ClassID { get; set; }

        public DateOnly Date { get; set; }

        public bool IsStudy { get; set; }

        public Guid? teacherAssignmentID { get; set; }
        public Guid? RoomID { get; set; }

        public string? RoomCode { get; set; }

        public string? CoveredTopic { get; set; }

        public string? CourseName { get; set; }
        public Guid? CourseId { get; set; }

        public string? TeacherName { get; set; }

        public string? OnlineMeetingUrl { get; set; }

        [StringLength(100)]
        public string? Passcode { get; set; }

        public string? RecordingUrl { get; set; }

        public string? ProgressNote { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public string slot { get; set; }
    }
}
