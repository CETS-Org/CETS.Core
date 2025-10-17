using DTOs.ACAD.ACAD_CourseBenefit.Responses;
using DTOs.ACAD.ACAD_CourseRequirement.Responses;
using DTOs.ACAD.ACAD_CourseSkill.Responses;
using DTOs.ACAD.ACAD_Syllabus.Responses;
using DTOs.IDN.IDN_Teacher.Responses;
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
        public string Duration { get; set; } = null!;
        public double Rating { get; set; }
        public int StudentsCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        
        // Additional detail fields
        public List<TeacherAcademicDetailResponse> TeacherDetails { get; set; } = new List<TeacherAcademicDetailResponse>();
        public List<SyllabusResponse> Syllabi { get; set; } = new List<SyllabusResponse>();
        public List<CourseBenefitResponse> Benefits { get; set; } = new List<CourseBenefitResponse>();
        public List<CourseRequirementResponse> Requirements { get; set; } = new List<CourseRequirementResponse>();
        public List<CourseSkillResponse> CourseSkills { get; set; } = new List<CourseSkillResponse>();
    }
    
   
}
