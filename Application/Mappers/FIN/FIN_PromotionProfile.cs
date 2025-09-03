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
			CreateMap<FIN_Promotion, PromotionResponse>().ReverseMap();
			CreateMap<CreatePromotionRequest, FIN_Promotion>();
			CreateMap<UpdatePromotionRequest, FIN_Promotion>();
		}
	}
}


