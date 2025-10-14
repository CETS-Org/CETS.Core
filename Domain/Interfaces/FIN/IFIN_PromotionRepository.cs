using Domain.Entities;

namespace Domain.Interfaces.FIN
{
    public interface IFIN_PromotionRepository : IBaseRepository<FIN_Promotion>
    {
        Task<IReadOnlyList<FIN_Promotion>> GetAllWithNavigationAsync();
        Task<FIN_Promotion?> GetByIdWithNavigationAsync(Guid id);
    }
}


