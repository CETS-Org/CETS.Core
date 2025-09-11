using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Course.Responses
{
    public class CourseDetailResponse : CourseResponse
    {
        public string CategoryName { get; set; } = null!;
        public string CourseLevel { get; set; } = null!;
        public string FormatName { get; set; } = null!;
        
        // Additional fields for detailed view
        public string Teacher { get; set; } = null!;
        public string Duration { get; set; } = null!;
        public double Rating { get; set; }
        public int StudentsCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Additional detail fields
        public List<string> Teachers { get; set; } = new List<string>();
        public List<SyllabusItemResponse> SyllabusItems { get; set; } = new List<SyllabusItemResponse>();
        public List<CourseBenefitItemResponse> Benefits { get; set; } = new List<CourseBenefitItemResponse>();
        public List<CourseRequirementItemResponse> Requirements { get; set; } = new List<CourseRequirementItemResponse>();
    }
    
    public class SyllabusItemResponse
    {
        public int SessionNumber { get; set; }
        public string TopicTitle { get; set; } = null!;
        public int? EstimatedMinutes { get; set; }
        public bool Required { get; set; }
        public string? Objectives { get; set; }
        public string? ContentSummary { get; set; }
    }

    public class CourseBenefitItemResponse
    {
        public Guid Id { get; set; }
        public Guid BenefitID { get; set; }
        public string BenefitName { get; set; } = null!;
    }

    public class CourseRequirementItemResponse
    {
        public Guid Id { get; set; }
        public Guid RequirementID { get; set; }
        public string RequirementName { get; set; } = null!;
    }
}
