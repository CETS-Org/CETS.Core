using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Course.Search
{
    public sealed class CourseSearchQuery
    {
        public string? Q { get; set; }                 // từ khoá
        public string? Sort { get; set; } = "Relevance"; // Relevance | Created.desc | Price.asc | Price.desc
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 18;

        // Filters
        public List<Guid> LevelIds { get; set; } = new();
        public List<Guid> CategoryIds { get; set; } = new();
        public List<Guid> SkillIds { get; set; } = new();
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
    }
}
