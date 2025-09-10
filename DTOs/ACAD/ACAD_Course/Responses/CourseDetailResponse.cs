using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Course.Responses
{
    public class CourseDetailResponse : CourseResponse
    {
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; } = null!;
        public string LevelName { get; set; } = null!;
        public string FormatName { get; set; } = null!;
        
        // Additional fields for detailed view
        public string Teacher { get; set; } = null!;
        public string Duration { get; set; } = null!;
        public double Rating { get; set; }
        public int StudentsCount { get; set; }
        public string Image { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Additional detail fields
        public List<string> Teachers { get; set; } = new List<string>();
        public List<SyllabusItemResponse> SyllabusItems { get; set; } = new List<SyllabusItemResponse>();
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
}
