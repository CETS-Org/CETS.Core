using Application.Interfaces.FIN;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.FIN;
using DTOs.FIN_Promotion.Requests;
using DTOs.FIN_Promotion.Responses;

namespace Application.Implementations.FIN
{
	public class FIN_PromotionService : BaseService<FIN_Promotion, PromotionResponse, UpdatePromotionRequest, CreatePromotionRequest>, IFIN_PromotionService
	{
		public FIN_PromotionService(IFIN_PromotionRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
			: base(repository, unitOfWork, mapper)
		{
		}
	}
}


