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
                .ForMember(dest => dest.CourseObjective, opt => opt.MapFrom(src => src.CourseObjective));

        }
    }
}
