using AutoMapper;
using Domain.Entities;
using DTOs.ACAD.ACAD_LearningMaterial.Requests;
using DTOs.ACAD.ACAD_LearningMaterial.Responses;

namespace Application.Mappers.ACAD
{
    public class ACAD_LearningMaterialProfile : Profile
    {
        public ACAD_LearningMaterialProfile()
        {
            // Request -> Entity
            CreateMap<CreateLearningMaterialRequest, ACAD_LearningMaterial>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StoreUrl, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<UpdateLearningMaterialRequest, ACAD_LearningMaterial>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.StoreUrl, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            // Entity -> Response
            CreateMap<ACAD_LearningMaterial, LearningMaterialResponse>()
                .ForMember(dest => dest.UploaderName, opt => opt.MapFrom(src => src.CreatedByNavigation  != null ? src.CreatedByNavigation.FullName : null))
                .ForMember(dest => dest.ClassMeetingDate, opt => opt.MapFrom(src => src.ClassMeeting != null ? src.ClassMeeting.Date : (DateOnly?)null))
                .ForMember(dest => dest.ClassMeetingSlot, opt => opt.MapFrom(src => src.ClassMeeting != null ? src.ClassMeeting.Slot.Name : null));
        }
    }
}
