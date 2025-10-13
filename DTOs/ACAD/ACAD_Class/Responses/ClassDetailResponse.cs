using System;

namespace DTOs.ACAD.ACAD_Class.Responses
{
    public class ClassDetailResponse
    {
        public Guid Id { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public Guid CourseId { get; set; }
        public int Capacity { get; set; }
        public int EnrolledCount { get; set; }
    }
}


