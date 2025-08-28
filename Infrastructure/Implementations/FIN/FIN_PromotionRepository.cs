using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.FIN;

namespace Infrastructure.Repositories.FIN
{
    public class FIN_PromotionRepository : BaseRepository<FIN_Promotion>, IFIN_PromotionRepository
    {
        public FIN_PromotionRepository(AppDbContext context) : base(context)
        {
        }
    }
}


