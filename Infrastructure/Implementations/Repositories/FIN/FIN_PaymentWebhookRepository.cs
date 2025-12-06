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

        public async Task<(IEnumerable<FIN_PaymentWebhook> Data, int TotalCount)> GetPaginatedAsync(
            string? eventType,
            string? accountName,
            DateTime? dateFrom,
            DateTime? dateTo,
            decimal? minAmount,
            decimal? maxAmount,
            int page,
            int pageSize)
        {
            var query = _context.Set<FIN_PaymentWebhook>()
                .Include(w => w.Payment)
                    .ThenInclude(p => p.Invoice)
                        .ThenInclude(i => i.FIN_InvoiceItems)
                            .ThenInclude(ii => ii.Course)
                .Include(w => w.Payment)
                    .ThenInclude(p => p.Invoice)
                        .ThenInclude(i => i.CreatedByNavigation)
                .Include(w => w.Gateway)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(eventType))
            {
                query = query.Where(w => w.EventType == eventType);
            }

            if (!string.IsNullOrEmpty(accountName))
            {
                query = query.Where(w => w.Payment.Invoice.CreatedByNavigation.FullName.Contains(accountName));
            }

            if (dateFrom.HasValue)
            {
                query = query.Where(w => w.ReceivedAt >= dateFrom.Value);
            }

            if (dateTo.HasValue)
            {
                query = query.Where(w => w.ReceivedAt <= dateTo.Value);
            }

            if (minAmount.HasValue)
            {
                query = query.Where(w => w.Payment.Amount >= minAmount.Value);
            }

            if (maxAmount.HasValue)
            {
                query = query.Where(w => w.Payment.Amount <= maxAmount.Value);
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply pagination and ordering
            var data = await query
                .OrderByDescending(w => w.ReceivedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalCount);
        }
    }
}


