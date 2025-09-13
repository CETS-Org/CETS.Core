using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Course.Search
{
    // Application/Courses/Search/CourseListItemDto.cs
    public sealed class CourseListItemDto
    {
        // BẮT BUỘC cho CourseCard + Course.ts
        public string Id { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string CourseImageUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string? CategoryName { get; set; }
        public string? CourseLevel { get; set; }        // e.g. Beginner/Intermediate/Advanced
        public string? FormatName { get; set; }         // e.g. Online/Hybrid/...

        public decimal StandardPrice { get; set; }

        // Cho Card
        public double Rating { get; set; } = 0;
        public int StudentsCount { get; set; } = 0;
        public string Duration { get; set; } = "—";
        public bool IsPopular { get; set; } = false;
        public bool IsNew { get; set; } = false;

        // Teacher
        public string? Teacher { get; set; }
        public string? TeacherFullName { get; set; }

        // Benefits (hiển thị 3 item đầu trên Card)
        public List<BenefitItem> Benefits { get; set; } = new();

        public sealed class BenefitItem
        {
            public string Id { get; set; } = string.Empty;
            public string BenefitName { get; set; } = string.Empty;
        }
    }

}
