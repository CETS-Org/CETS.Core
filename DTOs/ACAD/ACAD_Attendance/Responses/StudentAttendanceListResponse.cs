using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Attendance.Responses
{
    public class StudentAttendanceListResponse
    {
        public Guid StudentId { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AvatarUrl { get; set; }
        public Guid EnrollmentId { get; set; }
        public string EnrollmentStatus { get; set; } = string.Empty;
        
        // Thông tin điểm danh
        public Guid? AttendanceId { get; set; }
        public string AttendanceStatus { get; set; } = "Absent"; // Mặc định là Absent nếu chưa điểm danh
        public string? AttendanceNotes { get; set; }
        public bool HasAttended { get; set; } = false; // Đã được điểm danh chưa
    }
}

