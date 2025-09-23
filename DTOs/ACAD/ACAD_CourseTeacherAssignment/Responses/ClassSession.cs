using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_CourseTeacherAssignment.Responses
{
    public class ClassSession
    {
        public string slot { get; set; } = null!;
        public string RoomCode { get; set; } = null!;
        public string TopicName { get; set; } = null!;
        public int EnrolledCount { get; set; }
        public int Capacity { get; set; }
    }
}
