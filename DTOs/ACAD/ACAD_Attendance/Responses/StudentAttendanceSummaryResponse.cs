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
        public int TotalMeetings { get; set; }
        public int TotalPresent { get; set; }
        public int TotalAbsent { get; set; }
    }
}
