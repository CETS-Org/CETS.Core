using AutoMapper;
using Domain.Entities;
using DTOs.FIN_Promotion.Requests;
using DTOs.FIN_Promotion.Responses;

namespace Application.Mappers
{
	public class FIN_PromotionProfile : Profile
	{
		public FIN_PromotionProfile()
		{
			CreateMap<FIN_Promotion, PromotionResponse>().ReverseMap();
			CreateMap<CreatePromotionRequest, FIN_Promotion>();
			CreateMap<UpdatePromotionRequest, FIN_Promotion>();
		}
	}
}


