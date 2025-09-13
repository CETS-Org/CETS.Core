using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_CourseSkill.Requests;
using DTOs.ACAD.ACAD_CourseSkill.Responses;


namespace Application.Mappers.ACAD
{
    public class ACAD_CourseSkillProfile : Profile
    {
        public ACAD_CourseSkillProfile()
        {
            CreateMap<CreateSkillRequest, ACAD_CourseSkill>();
            CreateMap<UpdateCourseSkillRequest, ACAD_CourseSkill>();

            CreateMap<ACAD_CourseSkill, CourseSkillResponse>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))   
               .ForMember(dest => dest.CourseID, opt => opt.MapFrom(src => src.CourseID))
               .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course != null ? src.Course.CourseName : string.Empty))
               .ForMember(dest => dest.SkillID, opt => opt.MapFrom(src => src.SkillID))
               .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skill != null ? src.Skill.Name : string.Empty));
        }
    }
}
