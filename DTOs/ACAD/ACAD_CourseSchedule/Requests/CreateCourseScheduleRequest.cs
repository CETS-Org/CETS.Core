using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_CourseSchedule.Requests
{
    public class CreateCourseScheduleRequest
    {
        public Guid CourseID { get; set; }
        public Guid TimeSlotID { get; set; } // Time Slot ID
        public DayOfWeek DayOfWeek { get; set; }
    }
}
