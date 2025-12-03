using DTOs.ACAD.ACAD_CourseTeacherAssignment.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.IDN.IDN_Teacher.Requests
{
    public class GetAvailableTeachersRequest
    {
       
        public Guid CourseId { get; set; }

      
        public List<ClassScheduleInputDto> Schedules { get; set; } = new();

        public DateOnly StartDate { get; set; }

       
        public DateOnly EndDate { get; set; }
    }
}
