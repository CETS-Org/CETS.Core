using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_CourseTeacherAssignment.Requests
{
    public class GetAvailableTeachersRequest
    {
        public Guid CourseId { get; set; }

        
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public List<ClassScheduleInputDto> Schedules { get; set; } = new();
    }

    public class ClassScheduleInputDto
    {
        public DayOfWeek DayOfWeek { get; set; } 
        public Guid TimeSlotID { get; set; }
    }
}
