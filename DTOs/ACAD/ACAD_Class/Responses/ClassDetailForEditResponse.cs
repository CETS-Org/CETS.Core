using DTOs.ACAD.ACAD_Class.Requests;
using DTOs.IDN.IDN_Student.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Class.Responses
{
    public class ClassDetailForEditResponse
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public Guid? TeacherAssignmentID { get; set; }
        public string? TeacherName { get; set; }// TeacherAssignmentID
        public Guid? RoomId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; } = "Active"; // active, inactive, full

        // Danh sách lịch học (để hiển thị readonly)
        public List<ClassMeetingScheduleDto> Schedules { get; set; } = new();

        // Danh sách học viên đang trong lớp
        public List<WaitingStudentResponse> Enrollments { get; set; } = new();
    }
}
