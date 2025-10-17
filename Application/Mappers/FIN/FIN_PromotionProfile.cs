using AutoMapper;
using Domain.Entities;
using DTOs.FIN.FIN_Promotion.Requests;
using DTOs.FIN.FIN_Promotion.Responses;

namespace Application.Mappers.FIN
{
	public class FIN_PromotionProfile : Profile
	{
		public FIN_PromotionProfile()
		{
			CreateMap<FIN_Promotion, PromotionResponse>()
				.ForMember(dest => dest.CreatedByNavigation, opt => opt.MapFrom(src => src.CreatedByNavigation))
				.ForMember(dest => dest.UpdatedByNavigation, opt => opt.MapFrom(src => src.UpdatedByNavigation))
				.ForMember(dest => dest.PromotionType, opt => opt.MapFrom(src => src.PromotionType));

			CreateMap<IDN_Account, AccountSimpleResponse>()
				.ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

			CreateMap<CORE_LookUp, LookUpSimpleResponse>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => (string?)null));

			CreateMap<CreatePromotionRequest, FIN_Promotion>();
			CreateMap<UpdatePromotionRequest, FIN_Promotion>();
		}
	}
}


