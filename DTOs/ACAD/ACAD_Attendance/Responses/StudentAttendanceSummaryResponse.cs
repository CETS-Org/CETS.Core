using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Attendance.Responses
{
    public class StudentAttendanceSummaryResponse
    {
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string? ClassName { get; set; }
        public string? TeacherName { get; set; }
        public int TotalClasses { get; set; }
        public int TotalSessions { get; set; }
        public int TotalPresent { get; set; }
        public int TotalAbsent { get; set; }
        public int Attended { get; set; }
        public int Absent { get; set; }
        public double AttendanceRate { get; set; }
        public bool IsWarning { get; set; }
        public string? WarningMessage { get; set; }

        public List<AttendanceDetailResponse> SessionRecords { get; set; } = new();
    }
}
