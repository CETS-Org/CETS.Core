using System;
using System.Collections.Generic;

namespace DTOs.Analytics.ClassOverview.Responses
{
    /// <summary>
    /// Comprehensive analytics for a specific class
    /// </summary>
    public class ClassOverviewResponse
    {
        public Guid ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public Guid CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;

        /// <summary>
        /// Attendance & Activity Metrics
        /// Tỷ lệ chuyên cần, học viên vắng, tần suất buổi học
        /// </summary>
        public AttendanceActivityMetrics AttendanceActivity { get; set; } = new();

        /// <summary>
        /// Performance Metrics
        /// Tiến độ và điểm số trung bình theo thời gian hoặc học viên
        /// </summary>
        public ClassPerformanceMetrics Performance { get; set; } = new();

        /// <summary>
        /// Engagement Metrics
        /// Mức độ tương tác và phản hồi trong lớp
        /// </summary>
        public ClassEngagementMetrics Engagement { get; set; } = new();

        /// <summary>
        /// Operational Status
        /// Tình trạng lớp (đang học, kết thúc, hủy), số buổi học, tải lớp
        /// </summary>
        public ClassOperationalMetrics Operational { get; set; } = new();

        /// <summary>
        /// Teacher Effectiveness
        /// Đánh giá chất lượng giảng dạy cho từng lớp
        /// </summary>
        public TeacherEffectivenessMetrics TeacherEffectiveness { get; set; } = new();

        public DateTime GeneratedAt { get; set; } = DateTime.Now;
    }

    public class AttendanceActivityMetrics
    {
        /// <summary>
        /// Overall attendance rate (%)
        /// </summary>
        public decimal AttendanceRate { get; set; }

        /// <summary>
        /// Total number of meetings scheduled
        /// </summary>
        public int TotalMeetings { get; set; }

        /// <summary>
        /// Number of meetings completed
        /// </summary>
        public int CompletedMeetings { get; set; }

        /// <summary>
        /// Number of meetings remaining
        /// </summary>
        public int RemainingMeetings { get; set; }

        /// <summary>
        /// Average attendance per meeting
        /// </summary>
        public decimal AverageAttendancePerMeeting { get; set; }

        /// <summary>
        /// Patterns of absences (e.g., frequent absences, late arrivals)
        /// </summary>
        public List<AbsencePatternData> AbsencePatterns { get; set; } = new();

        /// <summary>
        /// Check-in trend over time
        /// </summary>
        public List<CheckInTrendData> CheckInTrend { get; set; } = new();

        /// <summary>
        /// Class density (how full is the class)
        /// </summary>
        public decimal ClassDensity { get; set; }

        /// <summary>
        /// Students with perfect attendance
        /// </summary>
        public int PerfectAttendanceCount { get; set; }

        /// <summary>
        /// Students with high absence rate (>30%)
        /// </summary>
        public int HighAbsenceCount { get; set; }
    }

    public class AbsencePatternData
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int TotalAbsences { get; set; }
        public decimal AbsenceRate { get; set; }
        public string Pattern { get; set; } = string.Empty; // "Frequent", "Occasional", "Rare"
        public DateTime? LastAbsenceDate { get; set; }
    }

    public class CheckInTrendData
    {
        public DateTime Date { get; set; }
        public int ExpectedAttendees { get; set; }
        public int ActualAttendees { get; set; }
        public decimal AttendanceRate { get; set; }
        public string MeetingStatus { get; set; } = string.Empty; // "Completed", "Cancelled", "Scheduled"
    }

    public class ClassPerformanceMetrics
    {
        /// <summary>
        /// Average score across all students and assignments
        /// </summary>
        public decimal AverageScore { get; set; }

        /// <summary>
        /// Completion rate (%)
        /// </summary>
        public decimal CompletionRate { get; set; }

        /// <summary>
        /// Pass rate (%)
        /// </summary>
        public decimal PassRate { get; set; }

        /// <summary>
        /// Number of students who completed the class
        /// </summary>
        public int CompletedStudents { get; set; }

        /// <summary>
        /// Number of students who dropped out
        /// </summary>
        public int DroppedStudents { get; set; }

        /// <summary>
        /// Progress achievement rate (%)
        /// </summary>
        public decimal ProgressAchievementRate { get; set; }

        /// <summary>
        /// Individual student performances
        /// </summary>
        public List<StudentPerformanceData> StudentPerformances { get; set; } = new();

        /// <summary>
        /// Performance trend over time
        /// </summary>
        public List<PerformanceTrendData> PerformanceTrend { get; set; } = new();

        /// <summary>
        /// Top performing students
        /// </summary>
        public List<TopStudentData> TopStudents { get; set; } = new();

        /// <summary>
        /// Students at risk (low performance)
        /// </summary>
        public List<AtRiskStudentData> AtRiskStudents { get; set; } = new();
    }

    public class StudentPerformanceData
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public decimal AverageScore { get; set; }
        public int CompletedAssignments { get; set; }
        public int TotalAssignments { get; set; }
        public decimal CompletionRate { get; set; }
        public string PerformanceStatus { get; set; } = string.Empty; // "Excellent", "Good", "Average", "Poor"
        public decimal AttendanceRate { get; set; }
    }

    public class PerformanceTrendData
    {
        public DateTime Period { get; set; }
        public decimal AverageScore { get; set; }
        public int CompletedAssignments { get; set; }
        public decimal CompletionRate { get; set; }
    }

    public class TopStudentData
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public decimal AverageScore { get; set; }
        public int Rank { get; set; }
    }

    public class AtRiskStudentData
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public decimal AverageScore { get; set; }
        public decimal AttendanceRate { get; set; }
        public int MissedAssignments { get; set; }
        public string RiskLevel { get; set; } = string.Empty; // "High", "Medium", "Low"
    }

    public class ClassEngagementMetrics
    {
        /// <summary>
        /// Overall participation level (%)
        /// </summary>
        public decimal ParticipationLevel { get; set; }

        /// <summary>
        /// Interaction rate (questions, discussions, etc.)
        /// </summary>
        public decimal InteractionRate { get; set; }

        /// <summary>
        /// Total feedback received
        /// </summary>
        public int FeedbackCount { get; set; }

        /// <summary>
        /// Average feedback rating
        /// </summary>
        public decimal AverageFeedbackRating { get; set; }

        /// <summary>
        /// Assignment submissions count
        /// </summary>
        public int AssignmentSubmissions { get; set; }

        /// <summary>
        /// Total assignments assigned
        /// </summary>
        public int TotalAssignments { get; set; }

        /// <summary>
        /// Assignment submission rate (%)
        /// </summary>
        public decimal AssignmentSubmissionRate { get; set; }

        /// <summary>
        /// On-time submission rate (%)
        /// </summary>
        public decimal OnTimeSubmissionRate { get; set; }

        /// <summary>
        /// Students with high engagement
        /// </summary>
        public int HighEngagementStudents { get; set; }

        /// <summary>
        /// Students with low engagement
        /// </summary>
        public int LowEngagementStudents { get; set; }
    }

    public class ClassOperationalMetrics
    {
        /// <summary>
        /// Class status: "Active", "Completed", "Cancelled", "Upcoming"
        /// </summary>
        public string ClassStatus { get; set; } = string.Empty;

        /// <summary>
        /// Maximum capacity
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Current enrolled count
        /// </summary>
        public int EnrolledCount { get; set; }

        /// <summary>
        /// Capacity utilization rate (%)
        /// </summary>
        public decimal CapacityUtilization { get; set; }

        /// <summary>
        /// Available spots
        /// </summary>
        public int AvailableSpots { get; set; }

        /// <summary>
        /// Class start date
        /// </summary>
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// Class end date
        /// </summary>
        public DateOnly EndDate { get; set; }

        /// <summary>
        /// Total lessons planned
        /// </summary>
        public int TotalLessons { get; set; }

        /// <summary>
        /// Lessons completed
        /// </summary>
        public int CompletedLessons { get; set; }

        /// <summary>
        /// Lessons remaining
        /// </summary>
        public int RemainingLessons { get; set; }

        /// <summary>
        /// Class duration in days
        /// </summary>
        public int ClassDurationDays { get; set; }

        /// <summary>
        /// Days elapsed since start
        /// </summary>
        public int DaysElapsed { get; set; }

        /// <summary>
        /// Days remaining until end
        /// </summary>
        public int DaysRemaining { get; set; }

        /// <summary>
        /// Class progress (%)
        /// </summary>
        public decimal ClassProgress { get; set; }
    }

    public class TeacherEffectivenessMetrics
    {
        /// <summary>
        /// Teacher ID
        /// </summary>
        public Guid? TeacherId { get; set; }

        /// <summary>
        /// Teacher name
        /// </summary>
        public string TeacherName { get; set; } = string.Empty;

        /// <summary>
        /// Average teacher rating from students
        /// </summary>
        public decimal TeacherRating { get; set; }

        /// <summary>
        /// Teacher punctuality score (%)
        /// </summary>
        public decimal TeacherPunctuality { get; set; }

        /// <summary>
        /// Total feedback count for this teacher in this class
        /// </summary>
        public int FeedbackCount { get; set; }

        /// <summary>
        /// Student progress impact (how much students improved)
        /// </summary>
        public decimal StudentProgressImpact { get; set; }

        /// <summary>
        /// Number of classes taught by this teacher
        /// </summary>
        public int TotalClassesTaught { get; set; }

        /// <summary>
        /// Average class completion rate under this teacher
        /// </summary>
        public decimal AverageClassCompletionRate { get; set; }

        /// <summary>
        /// Teacher attendance rate (%)
        /// </summary>
        public decimal TeacherAttendanceRate { get; set; }
    }

    /// <summary>
    /// Summary response for class list
    /// </summary>
    public class ClassSummaryResponse
    {
        public Guid ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public Guid CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string ClassStatus { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int EnrolledCount { get; set; }
        public decimal CapacityUtilization { get; set; }
        public decimal AttendanceRate { get; set; }
        public decimal AverageScore { get; set; }
        public decimal CompletionRate { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string? TeacherName { get; set; }
        public DateTime GeneratedAt { get; set; }
    }

    /// <summary>
    /// Paginated response for class list
    /// </summary>
    public class ClassListResponse
    {
        public List<ClassSummaryResponse> Classes { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}



