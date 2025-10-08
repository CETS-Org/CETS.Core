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
    }
}


