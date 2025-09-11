using DTOs.ACAD.ACAD_CourseBenefit.Responses;
using DTOs.ACAD.ACAD_CourseRequirement.Responses;
using DTOs.ACAD.ACAD_SyllabusItem.Responses;
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
        
        // Additional detail fields
        public TeacherAcademicDetailResponse TeacherDetail { get; set; } = new TeacherAcademicDetailResponse();
        public List<SyllabusItemResponse> SyllabusItems { get; set; } = new List<SyllabusItemResponse>();
        public List<CourseBenefitResponse> Benefits { get; set; } = new List<CourseBenefitResponse>();
        public List<CourseRequirementResponse> Requirements { get; set; } = new List<CourseRequirementResponse>();
    }
    
   
}
