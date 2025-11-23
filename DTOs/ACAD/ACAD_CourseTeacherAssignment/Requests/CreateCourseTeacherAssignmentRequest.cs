using System;

namespace DTOs.ACAD.ACAD_CourseTeacherAssignment.Requests
{
    public class CreateCourseTeacherAssignmentRequest
    {
        public Guid CourseID { get; set; }
        public Guid TeacherID { get; set; }
    }
}

