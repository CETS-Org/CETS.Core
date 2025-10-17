using System;
using System.Collections.Generic;

namespace DTOs.ACAD.ACAD_Class.Responses
{
    public class ClassRowResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string CourseId { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string Teacher { get; set; } = string.Empty;
        public string Room { get; set; } = string.Empty;
        public int CurrentStudents { get; set; }
        public int MaxStudents { get; set; }
        public string Status { get; set; } = string.Empty; // "active" | "inactive" | "full"
        public List<ClassScheduleItem>? Schedule { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
    }
}


