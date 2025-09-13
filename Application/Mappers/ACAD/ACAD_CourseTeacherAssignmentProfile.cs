using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_CourseTeacherAssignment.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.ACAD
{
    public class ACAD_CourseTeacherAssignmentProfile : Profile
    {
        public ACAD_CourseTeacherAssignmentProfile()
        {
            CreateMap<ACAD_CourseTeacherAssignment, CourseListAssignmentResponse>()
             .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.Course.Id))
             .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.Course.CourseCode))
             .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.CourseName))
             .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Course.Description))
             .ForMember(dest => dest.CourseImageUrl, opt => opt.MapFrom(src => src.Course.CourseImageUrl))
             .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Course.Category.Name))
             .ForMember(dest => dest.CourseLevelName, opt => opt.MapFrom(src => src.Course.CourseLevel.Name))
             .ForMember(dest => dest.CourseFormatName, opt => opt.MapFrom(src => src.Course.CourseFormat.Name))
             .ForMember(dest => dest.StudentCount, opt => opt.MapFrom(src => src.Course.ACAD_Enrollments.Count))
             .ForMember(dest => dest.AssignedAt, opt => opt.MapFrom(src => src.Course.ACAD_CourseTeacherAssignments 
                                                            .OrderByDescending(a => a.AssignedAt)
                                                            .FirstOrDefault()!.AssignedAt));

            CreateMap<ACAD_Course, CourseListAssignmentResponse>()
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.CourseCode))
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.CourseName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.CourseImageUrl, opt => opt.MapFrom(src => src.CourseImageUrl))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.CourseLevelName, opt => opt.MapFrom(src => src.CourseLevel.Name))
            .ForMember(dest => dest.CourseFormatName, opt => opt.MapFrom(src => src.CourseFormat.Name))
            .ForMember(dest => dest.StudentCount, opt => opt.MapFrom(src => src.ACAD_Enrollments.Count));
        }
    }
}
