using System;
using System.Collections.Generic;
using DTOs.IDN.IDN_Teacher.Responses;

namespace DTOs.ACAD.ACAD_CourseWishlist.Responses
{
    public class WishlistItemResponse
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? CourseImageUrl { get; set; }
        public decimal StandardPrice { get; set; }
        public string CourseLevel { get; set; } = string.Empty;
        public string CourseFormat { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public double Rating { get; set; }
        public int StudentsCount { get; set; }
        public List<TeacherAcademicDetailResponse> TeacherDetails { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }
}

