using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Attendance.Responses
{
    public class BulkAttendanceResponse
    {
        public Guid ClassMeetingId { get; set; }
        public int TotalStudents { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public DateTime MarkedAt { get; set; }
        public string MarkedByTeacher { get; set; } = string.Empty;
        public List<AttendanceRecordResponse> Records { get; set; } = new List<AttendanceRecordResponse>();
    }

    public class AttendanceRecordResponse
    {
        public Guid AttendanceId { get; set; }
        public Guid StudentId { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}


