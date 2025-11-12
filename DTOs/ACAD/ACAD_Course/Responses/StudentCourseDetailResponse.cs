using DTOs.ACAD.ACAD_Assignment.Responses;
using DTOs.ACAD.ACAD_ClassMeetings.Responses;
using DTOs.ACAD.ACAD_Submission.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Course.Responses
{
    public class StudentCourseDetailResponse
    {
        public Guid CourseId { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public List<string> TeacherNames { get; set; } = new();
        public string StatusCode { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;

        public List<ClassMeetingAssignmentResponse> Assignments { get; set; } = new();
        public List<WeeklySubmissionPerformanceResponse> WeeklyPerformance { get; set; } = new();
        public List<ClassMeetingAssignmentResponse> ClassMeetings { get; set; } = new();
        public AssignmentCompletionStatsResponse? CompletionStats { get; set; }


    }
}
