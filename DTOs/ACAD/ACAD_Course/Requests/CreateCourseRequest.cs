using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Course.Requests
{
    public class CreateCourseRequest
    {
        public string CourseCode { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public Guid CourseLevelID { get; set; }
        public Guid CourseFormatID { get; set; }
        public string? CourseImageUrl { get; set; }
        public List<string>? CourseObjective { get; set; } = new();
        public Guid CategoryID { get; set; }
        public string? Description { get; set; }
        public decimal StandardPrice { get; set; }
        public decimal StandardScore { get; set; }
        public decimal ExitScore { get; set; }
        public bool IsActive { get; set; } = true;

        // Related details
        public List<Guid>? BenefitIDs { get; set; } = new();
        public List<Guid>? RequirementIDs { get; set; } = new();
        public List<Guid>? SkillIDs { get; set; } = new();
        public List<CreateCourseScheduleDetail>? Schedules { get; set; } = new();
        public List<CreateCourseSyllabusDetail>? Syllabi { get; set; } = new();
    }

    public class CreateCourseScheduleDetail
    {
        public Guid TimeSlotID { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }

    public class CreateCourseSyllabusDetail
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<CreateCourseSyllabusItemDetail>? Items { get; set; } = new();
    }

    public class CreateCourseSyllabusItemDetail
    {
        public int SessionNumber { get; set; }
        public string TopicTitle { get; set; } = string.Empty;
        public int? TotalSlots { get; set; }
        public bool Required { get; set; } = true;
        public string? Objectives { get; set; }
        public string? ContentSummary { get; set; }
        public string? PreReadingUrl { get; set; }
    }
}
