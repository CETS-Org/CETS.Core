using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_Course.Requests;
using DTOs.ACAD.ACAD_Course.Responses;
using DTOs.ACAD.ACAD_CourseBenefit.Responses;
using DTOs.ACAD.ACAD_CourseRequirement.Responses;
using DTOs.ACAD.ACAD_SyllabusItem.Responses;
using DTOs.IDN.IDN_Teacher.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.ACAD
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<CreateCourseRequest, ACAD_Course>()
                .ForMember(dest => dest.CourseObjective, opt => opt.MapFrom(src => src.CourseObjective));
            
            CreateMap<UpdateCourseRequest, ACAD_Course>()
                .ForMember(dest => dest.CourseObjective, opt => opt.MapFrom(src => src.CourseObjective));

            CreateMap<ACAD_Course, CourseResponse>()
                .ForMember(dest => dest.CourseObjective, opt => opt.MapFrom(src => src.CourseObjective));

            CreateMap<ACAD_Course, CourseDetailResponse>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : ""))
                .ForMember(dest => dest.CourseLevel, opt => opt.MapFrom(src => src.CourseLevel != null ? src.CourseLevel.Name : ""))
                .ForMember(dest => dest.FormatName, opt => opt.MapFrom(src => src.CourseFormat != null ? src.CourseFormat.Name : ""))
                .ForMember(dest => dest.CourseObjective, opt => opt.MapFrom(src => src.CourseObjective))
                
                // Teacher information
                .ForMember(dest => dest.TeacherDetails, opt => opt.MapFrom(src => 
                    src.ACAD_CourseTeacherAssignments.Select(a => a.Teacher).ToList()))
                
                // Course statistics
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => 
                    (src.ACAD_Syllabi.SelectMany(s => s.ACAD_SyllabusItems).Sum(i => i.EstimatedMinutes ?? 0) / 60.0).ToString("0.0") + " hours"))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => 
                    src.COM_Feedbacks.Any() ? src.COM_Feedbacks.Average(f => (double?)f.Rating) ?? 0.0 : 0.0))
                .ForMember(dest => dest.StudentsCount, opt => opt.MapFrom(src => 
                    src.ACAD_Enrollments.Count(e => !e.IsDeleted)))
                
                // Detailed course content
                .ForMember(dest => dest.SyllabusItems, opt => opt.MapFrom(src => 
                    src.ACAD_Syllabi.SelectMany(s => s.ACAD_SyllabusItems).OrderBy(i => i.SessionNumber)))
                .ForMember(dest => dest.Benefits, opt => opt.MapFrom(src => src.ACAD_CourseBenefits))
                .ForMember(dest => dest.Requirements, opt => opt.MapFrom(src => src.ACAD_CourseRequirements));

            CreateMap<ACAD_Course, CourseListItemResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.TeacherDetails, opt => opt.MapFrom(src => 
                    src.ACAD_CourseTeacherAssignments.Select(a => a.Teacher).ToList()))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => 
                    (src.ACAD_Syllabi.SelectMany(s => s.ACAD_SyllabusItems).Sum(i => i.EstimatedMinutes ?? 0) / 60.0).ToString("0.0") + " hours"))
                .ForMember(dest => dest.CourseLevel, opt => opt.MapFrom(src => src.CourseLevel.Name))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => 
                    src.COM_Feedbacks.Any() ? src.COM_Feedbacks.Average(f => (double?)f.Rating) ?? 0.0 : 0.0))
                .ForMember(dest => dest.StudentsCount, opt => opt.MapFrom(src => 
                    src.ACAD_Enrollments.Count(e => !e.IsDeleted)))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            // Nested object mappings
            CreateMap<IDN_Teacher, TeacherAcademicDetailResponse>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Account.FullName))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => 
                    src.COM_Feedbacks.Any(f => f.Rating.HasValue) ? 
                    Math.Round(src.COM_Feedbacks.Where(f => f.Rating.HasValue).Average(f => (double)f.Rating!.Value), 1) : 0.0))
                .ForMember(dest => dest.TotalStudents, opt => opt.Ignore())
                .ForMember(dest => dest.TotalCourses, opt => opt.Ignore());

            CreateMap<ACAD_SyllabusItem, SyllabusItemResponse>();

            CreateMap<ACAD_CourseBenefit, CourseBenefitResponse>()
                .ForMember(dest => dest.BenefitName, opt => opt.MapFrom(src => src.Benefit.Name));

            CreateMap<ACAD_CourseRequirement, CourseRequirementResponse>()
                .ForMember(dest => dest.RequirementName, opt => opt.MapFrom(src => src.Requirement.Name));

        }
    }
}
