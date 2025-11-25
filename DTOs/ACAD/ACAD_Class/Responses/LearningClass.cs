using DTOs.ACAD.ACAD_ClassMeetings.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Class.Responses
{
    public class LearningClassResponse
    {
        public Guid Id { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public string? CourseName {  get; set; }
        public string? CourseCode { get; set; }
        public string? ClassName { get; set; }
        public Guid? TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string? TimeSlot {  get; set; }
        public string? RoomCode { get; set; }
        public bool IsActive { get; set; }
        public ClassMeetingResponse? nextMeeting { get;set;}
    }
}
