using System;

namespace DTOs.ACAD.ACAD_CourseTeacherAssignment.Responses
{
    public class CourseTeacherAssignmentResponse
    {
        public Guid AssignmentId { get; set; }
        public Guid CourseID { get; set; }
        public Guid TeacherID { get; set; }
        public DateTime AssignedAt { get; set; }

        public string? TeacherName { get; set; }
        public string? TeacherEmail { get; set; }
        public string? TeacherAvatarUrl { get; set; }
        public string? TeacherCode { get; set; }
    }
}

