using System;
using System.Collections.Generic;

namespace DTOs.ACAD.ACAD_CourseSchedule.Responses
{
    public class WeeklyScheduleResponse
    {
        public int DayOfWeek { get; set; } // 2-7 (Monday-Sunday in database)
        public string DayName { get; set; } = string.Empty;
        public List<TimeSlotInfo> TimeSlots { get; set; } = new();
    }

    public class TimeSlotInfo
    {
        public Guid TimeSlotID { get; set; }
        public string TimeSlotCode { get; set; } = string.Empty; // slot1, slot2, etc.
        public string TimeSlotName { get; set; } = string.Empty; // 09:00, 13:30, etc.
        public List<CourseInSchedule> Courses { get; set; } = new();
    }

    public class CourseInSchedule
    {
        public Guid CourseID { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
    }
}
