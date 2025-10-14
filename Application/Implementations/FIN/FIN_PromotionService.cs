using Application.Interfaces.FIN;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.FIN;
using DTOs.FIN.FIN_Promotion.Requests;
using DTOs.FIN.FIN_Promotion.Responses;

namespace Application.Implementations.FIN
{
	public class FIN_PromotionService : BaseService<FIN_Promotion, PromotionResponse, UpdatePromotionRequest, CreatePromotionRequest>, IFIN_PromotionService
	{
		private readonly IFIN_PromotionRepository _promotionRepository;

		public FIN_PromotionService(IFIN_PromotionRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
			: base(repository, unitOfWork, mapper)
		{
			_promotionRepository = repository;
		}

		public override async Task<IReadOnlyList<PromotionResponse>> GetAllAsync()
		{
			var entities = await _promotionRepository.GetAllWithNavigationAsync();
			return _mapper.Map<IReadOnlyList<PromotionResponse>>(entities);
		}

		public override async Task<PromotionResponse?> GetByIdAsync(Guid id)
		{
			var entity = await _promotionRepository.GetByIdWithNavigationAsync(id);
			return _mapper.Map<PromotionResponse?>(entity);
		}
	}
}


