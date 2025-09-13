using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseEntity = Domain.Entities.ACAD_Course;

namespace DTOs.ACAD.ACAD_Course.Search
{
    public static class CourseMapping
    {
        public static CourseListItemDto ToListItem(this CourseEntity c)
        {
          double rating = c.COM_Feedbacks?.Count > 0
            ? c.COM_Feedbacks!.Average(f => (double?)f.Rating) ?? 0
            : 0;

        int students = c.ACAD_Enrollments?.Count ?? 0;

            var durationMinutes = c.ACAD_Syllabi
            .SelectMany(s => s.ACAD_SyllabusItems)
            .Where(i => !i.IsDeleted && i.EstimatedMinutes.HasValue)
            .Sum(i => i.EstimatedMinutes.Value);


            

            bool isNew = (DateTime.UtcNow - c.CreatedAt).TotalDays <= 30;
            bool isPopular = students >= 100;

        var benefits = c.ACAD_CourseBenefits?
            .Take(3)
            .Select(b => new CourseListItemDto.BenefitItem
            {
                Id = b.BenefitID.ToString(),
                BenefitName = b.Benefit?.Name ?? string.Empty
            })
            .ToList() ?? new();

        return new CourseListItemDto
        {
            Id = c.Id.ToString(),
            CourseCode = c.CourseCode ?? string.Empty,
            CourseName = c.CourseName,
            CourseImageUrl = string.IsNullOrEmpty(c.CourseImageUrl)
                ? "https://images.unsplash.com/photo-1434030216411-0b793f4b4173?w=400&h=250&fit=crop"
                : c.CourseImageUrl!,
            Description = c.Description ?? string.Empty,

            CategoryName = c.Category?.Name,
            CourseLevel = c.CourseLevel?.Name,
            FormatName = c.CourseFormat?.Name,

            StandardPrice = c.StandardPrice,

            Rating = Math.Round(rating, 1),
            StudentsCount = students,
            Duration = durationMinutes > 0
            ? $"{durationMinutes / 60}h {durationMinutes % 60}m"
            : "N/A",
            IsNew = isNew,
            IsPopular = isPopular,

            Teacher = null,
            TeacherFullName = c.ACAD_CourseTeacherAssignments?
                                  .Select(t => t.Teacher.Account.FullName)
                                  .FirstOrDefault(),
            Benefits = benefits
        };
        }
    }
}
