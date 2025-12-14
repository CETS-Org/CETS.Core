using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Course.Responses
{
    public class UpcomingCourseEnrollmentInfo
    {
        public Guid EnrollmentId { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string StudentEmail { get; set; } = string.Empty;
        public string ClassCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public string RoomName { get; set; } = string.Empty; // Nếu có
        public string TimeSlot { get; set; } = string.Empty; // Ca học
    }
}
