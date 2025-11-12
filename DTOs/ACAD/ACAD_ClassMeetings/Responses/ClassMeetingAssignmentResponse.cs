using DTOs.ACAD.ACAD_Assignment.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_ClassMeetings.Responses
{
    public class ClassMeetingAssignmentResponse
    {
        public Guid MeetingId { get; set; }
        public DateTime MeetingDate { get; set; }
        public string Topic { get; set; } = string.Empty;
        public List<StudentAssignmentResponse> Assignments { get; set; } = new();
    }
}
