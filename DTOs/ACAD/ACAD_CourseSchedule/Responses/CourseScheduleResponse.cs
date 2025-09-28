using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_CourseSchedule.Responses
{
    public class CourseScheduleResponse
    {
        public Guid Id { get; set; }
        public Guid CourseID { get; set; }
        public Guid TimeSlotID { get; set; }
        public string DayOfWeek { get; set; } = null!;
        public string? CourseName { get; set; }
        public string? TimeSlotName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
