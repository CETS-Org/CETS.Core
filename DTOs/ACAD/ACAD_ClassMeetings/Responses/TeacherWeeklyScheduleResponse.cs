using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_ClassMeetings.Responses
{
    public class TeacherWeeklyScheduleResponse
    {
        public DateTime Date { get; set; }
        public string DayOfWeek { get; set; } = string.Empty;
        public string Slot { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string? Room { get; set; }
        public int EnrolledCount { get; set; }
        public int Capacity { get; set; }
        public string? OnlineMeetingUrl { get; set; }
    }
}

