using System;

namespace DTOs.ACAD.ACAD_Course.Responses
{
    public class CourseScheduleInfo
    {
        public int DayOfWeek { get; set; } // 2-7 (Monday-Sunday)
        public string DayName { get; set; } = string.Empty;
        public string TimeSlotCode { get; set; } = string.Empty; // slot1, slot2, etc.
        public string TimeSlotName { get; set; } = string.Empty; // 09:00, 13:30, etc.
    }
}
