using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Attendance.Responses
{
    public class AttendanceResponse
    {
        public Guid AttendanceId { get; set; }
        public Guid MeetingId { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;

        public Guid StatusId { get; set; }
        public string StatusName { get; set; } = string.Empty;

        public string? Notes { get; set; }
        public Guid? CheckedBy { get; set; }
        public string? CheckedByName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
