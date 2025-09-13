using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Course.Search
{
    // Application/Courses/Search/CourseSearchResult.cs
    public sealed class CourseSearchResult
    {
        public List<CourseListItemDto> Items { get; set; } = new();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }

        public Dictionary<string, List<FacetItem>> Facets { get; set; } = new();
        public sealed class FacetItem
        {
            public string Key { get; set; } = string.Empty;      // Id (Guid.ToString) hoặc text
            public string? Label { get; set; }                   // hiển thị
            public int Count { get; set; }
            public bool Selected { get; set; }
        }
    }

}
