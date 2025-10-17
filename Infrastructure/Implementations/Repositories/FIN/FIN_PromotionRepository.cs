using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.FIN;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.FIN
{
    public class FIN_PromotionRepository : BaseRepository<FIN_Promotion>, IFIN_PromotionRepository
    {
        public FIN_PromotionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<FIN_Promotion>> GetAllWithNavigationAsync()
        {
            return await _context.Set<FIN_Promotion>()
                .Include(p => p.CreatedByNavigation)
                .Include(p => p.UpdatedByNavigation)
                .Include(p => p.PromotionType)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<FIN_Promotion?> GetByIdWithNavigationAsync(Guid id)
        {
            return await _context.Set<FIN_Promotion>()
                .Include(p => p.CreatedByNavigation)
                .Include(p => p.UpdatedByNavigation)
                .Include(p => p.PromotionType)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}


