using System;
using System.Collections.Generic;

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
        
        // Teacher information
        public Guid? TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        
        // Schedule and location
        public string Schedule { get; set; } = string.Empty;
        public string Room { get; set; } = string.Empty;
        
        // Dates
        public string StartDate { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
        
        // Status
        public string Status { get; set; } = string.Empty;
        public string StatusCode { get; set; } = string.Empty;
        
        // Description
        public string? Description { get; set; }
        
        // Progress tracking
        public int TotalSessions { get; set; }
        public int CompletedSessions { get; set; }
        
        // Students list
        public List<StudentInClassResponse> Students { get; set; } = new List<StudentInClassResponse>();
    }
}


