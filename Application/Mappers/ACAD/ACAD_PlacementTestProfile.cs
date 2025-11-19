using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_PlacementTest.Requests;
using DTOs.ACAD.ACAD_PlacementTest.Responses;

namespace Application.Mappers.ACAD
{
    public class ACAD_PlacementTestProfile : Profile
    {
        public ACAD_PlacementTestProfile()
        {
            // Map PlacementQuestion
            CreateMap<ACAD_PlacementQuestion, PlacementQuestionResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SkillTypeID, opt => opt.MapFrom(src => src.SkillTypeID))
                .ForMember(dest => dest.SkillType, opt => opt.MapFrom(src => src.Skill != null ? src.Skill.Name : ""))
                .ForMember(dest => dest.QuestionTypeID, opt => opt.MapFrom(src => src.QuestionTypeID))
                .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.QuestionType != null ? src.QuestionType.Name : ""))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.QuestionUrl, opt => opt.MapFrom(src => src.QuestionUrl))
                .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src => src.Difficulty))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));

            // Map PlacementTest entity to responses
            CreateMap<ACAD_PlacementTest, PlacementTestResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.DurationMinutes, opt => opt.MapFrom(src => src.DurationMinutes))
                .ForMember(dest => dest.StoreUrl, opt => opt.MapFrom(src => src.StoreUrl))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.Questions, opt => opt.Ignore());

            CreateMap<ACAD_PlacementTest, CreatePlacementTestResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.DurationMinutes, opt => opt.MapFrom(src => src.DurationMinutes))
                .ForMember(dest => dest.StoreUrl, opt => opt.MapFrom(src => src.StoreUrl))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UploadUrl, opt => opt.Ignore())
                .ForMember(dest => dest.QuestionJson, opt => opt.Ignore());
        }
    }
}

