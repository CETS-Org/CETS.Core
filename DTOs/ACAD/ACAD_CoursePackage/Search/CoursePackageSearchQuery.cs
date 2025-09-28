using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_CoursePackage.Search
{
    public sealed class CoursePackageSearchQuery
    {
        public string? Q { get; set; }                 // từ khoá
        public string? Sort { get; set; } = "Relevance"; // Relevance | Created.desc | Price.asc | Price.desc
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 18;

        // Filters
        public List<Guid> LevelIds { get; set; } = new();
        public List<Guid> CategoryIds { get; set; } = new();
        public List<Guid> SkillIds { get; set; } = new();
        // Schedule filters (by course schedules within packages)
        public List<string> DaysOfWeek { get; set; } = new(); // e.g., "Monday", "Tuesday"
        public List<Guid> TimeSlotIds { get; set; } = new();   // CORE_LookUp IDs for time slots
        public List<string> TimeSlotNames { get; set; } = new(); // fallback by name
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
        public bool? IsActive { get; set; }
        public int? MinCourseCount { get; set; }
        public int? MaxCourseCount { get; set; }
    }
}

