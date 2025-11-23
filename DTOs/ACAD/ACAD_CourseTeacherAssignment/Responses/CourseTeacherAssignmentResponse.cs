using System;

namespace DTOs.ACAD.ACAD_CourseTeacherAssignment.Responses
{
    public class CourseTeacherAssignmentResponse
    {
        public Guid Id { get; set; }
        public Guid AssignmentId { get; set; }
        public Guid CourseID { get; set; }
        public Guid TeacherID { get; set; }
        public DateTime AssignedAt { get; set; }

        // Properties from HEAD branch
        public string? CourseName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? AvatarUrl { get; set; }
        public int? YearsExperience { get; set; }

        // Properties from dev_minhdq branch
        public string? TeacherName { get; set; }
        public string? TeacherEmail { get; set; }
        public string? TeacherAvatarUrl { get; set; }
        public string? TeacherCode { get; set; }
    }
}
