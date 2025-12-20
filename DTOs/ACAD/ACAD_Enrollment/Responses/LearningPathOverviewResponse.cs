using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Enrollment.Responses
{
    public class LearningPathOverviewResponse
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public OverallStatsResponse OverallStats { get; set; } = new();
        public List<CourseOverviewItemResponse> Courses { get; set; } = new();
    }

    public class OverallStatsResponse
    {
        public int TotalCourses { get; set; }
        public int PassedCourses { get; set; }
        public int FailedCourses { get; set; }
        public int InProgressCourses { get; set; }
        public double OverallAttendanceRate { get; set; }
        public int TotalSessions { get; set; }
        public int TotalAttended { get; set; }
        public int TotalAbsent { get; set; }
    }

    public class CourseOverviewItemResponse
    {
        public Guid CourseId { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public List<string> TeacherNames { get; set; } = new();
        public string StatusCode { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;
        public string CourseProgress { get; set; } = string.Empty;
        public string Instructor { get; set; } = string.Empty;

        public DateTime? ExpectedStartDate { get; set; }
    }

}
