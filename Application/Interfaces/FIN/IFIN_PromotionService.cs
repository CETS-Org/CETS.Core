using Domain.Entities;
using DTOs.FIN.FIN_Promotion.Requests;
using DTOs.FIN.FIN_Promotion.Responses;

namespace Application.Interfaces.FIN
{
	public interface IFIN_PromotionService : IBaseService<FIN_Promotion, PromotionResponse, UpdatePromotionRequest, CreatePromotionRequest>
	{
	}
}


