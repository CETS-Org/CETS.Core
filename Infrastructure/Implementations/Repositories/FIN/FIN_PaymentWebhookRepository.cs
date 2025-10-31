using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.FIN;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.FIN
{
    public class FIN_PaymentWebhookRepository : BaseRepository<FIN_PaymentWebhook>, IFIN_PaymentWebhookRepository
    {
        public FIN_PaymentWebhookRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<FIN_PaymentWebhook>> GetAllWithDetailsAsync()
        {
            return await _context.Set<FIN_PaymentWebhook>()
                .Include(w => w.Payment)
                    .ThenInclude(p => p.Invoice)
                        .ThenInclude(i => i.FIN_InvoiceItems)
                            .ThenInclude(ii => ii.Course)
                .Include(w => w.Payment)
                    .ThenInclude(p => p.Invoice)
                        .ThenInclude(i => i.CreatedByNavigation)
                .Include(w => w.Gateway)
                .OrderByDescending(w => w.CreatedAt)
                .ToListAsync();
        }
    }
}


