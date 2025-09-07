using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_Course.Requests;
using DTOs.ACAD.ACAD_Course.Responses;
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
            CreateMap<CreateCourseRequest, ACAD_Course>();
            CreateMap<UpdateCourseRequest, ACAD_Course>();

            CreateMap<ACAD_Course, CourseResponse>();
            CreateMap<ACAD_Course, CourseDetailResponse>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : ""))
                .ForMember(dest => dest.LevelName, opt => opt.MapFrom(src => src.CourseLevelID.ToString()))
                .ForMember(dest => dest.FormatName, opt => opt.MapFrom(src => src.CourseFormatID.ToString()));
        }
    }
}
