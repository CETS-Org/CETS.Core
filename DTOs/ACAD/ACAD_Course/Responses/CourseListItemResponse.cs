using DTOs.ACAD.ACAD_CourseSkill.Responses;
using DTOs.IDN.IDN_Teacher.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Course.Responses
{
    public class CourseListItemResponse
    {
        public string Id { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public string? Description { get; set; }
        public List<string>? CourseObjective { get; set; } = new();
        public string Duration { get; set; } = null!;
        public string CourseLevel { get; set; } = null!;
        public decimal StandardPrice { get; set; }
        public double Rating { get; set; }
        public int StudentsCount { get; set; }
        public string CourseImageUrl { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public List<TeacherAcademicDetailResponse> TeacherDetails { get; set; } = new List<TeacherAcademicDetailResponse>();
        public List<CourseSkillResponse> CourseSkills { get; set; } = new List<CourseSkillResponse>();

    }
}
