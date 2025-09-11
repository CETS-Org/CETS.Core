using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_CourseRequirement.Requests;
using DTOs.ACAD.ACAD_CourseRequirement.Responses;

namespace Application.Mappers.ACAD
{
    public class CourseRequirementProfile : Profile
    {
        public CourseRequirementProfile()
        {
            CreateMap<CreateCourseRequirementRequest, ACAD_CourseRequirement>();
            CreateMap<UpdateCourseRequirementRequest, ACAD_CourseRequirement>();

            CreateMap<ACAD_CourseRequirement, CourseRequirementResponse>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course != null ? src.Course.CourseName : ""))
                .ForMember(dest => dest.RequirementName, opt => opt.MapFrom(src => src.Requirement != null ? src.Requirement.Name : ""));
        }
    }
}
