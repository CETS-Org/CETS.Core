using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.FIN;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.FIN
{
    public class FIN_InvoiceItemRepository : BaseRepository<FIN_InvoiceItem>, IFIN_InvoiceItemRepository
    {
        public FIN_InvoiceItemRepository(AppDbContext context) : base(context)
        {
        }
        
        public async Task<IEnumerable<FIN_InvoiceItem>> GetByInvoiceIdAsync(Guid invoiceId)
        {
            return await _context.FIN_InvoiceItems
                .Include(i => i.Course)
                .Where(ii => ii.InvoiceID == invoiceId)
                .OrderBy(ii => ii.Id)
                .ToListAsync();
        }

        public async Task<List<FIN_InvoiceItem>> GetByInvoiceIdsAsync(List<Guid> invoiceIds)
        {
            if (invoiceIds == null || !invoiceIds.Any())
            {
                return new List<FIN_InvoiceItem>();
            }

        
            return await _context.FIN_InvoiceItems
                                 .Where(x => invoiceIds.Contains(x.InvoiceID))
                                 .ToListAsync();
        }
    }
}


