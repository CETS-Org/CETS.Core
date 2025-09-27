using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_CourseTeacherAssignment.Responses
{
    public class ClassSession
    {
        public Guid ClassMeetingsId { get; set; }
        public string slot { get; set; } = null!;
        public string RoomCode { get; set; } = null!;
        public string TopicName { get; set; } = null!;
        public DateOnly Date { get; set; }
        public bool isStudyingDay { get; set; }
    }
}
