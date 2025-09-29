using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Attendance.Responses
{
    public class AttendanceDetailResponse
    {
        public Guid MeetingId { get; set; }
        public DateTime MeetingDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }

        public string TopicTitle { get; set; } = string.Empty;
        public string? RoomCode { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? CheckedBy { get; set; }
    }
}
