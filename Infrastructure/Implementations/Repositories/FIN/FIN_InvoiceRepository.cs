using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.FIN;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.FIN
{
    public class FIN_InvoiceRepository : BaseRepository<FIN_Invoice>, IFIN_InvoiceRepository
    {
        public FIN_InvoiceRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<int> GetNextSequenceInvoiceIdAsync()
        {
            return (int)(await _context.FIN_Invoices
                    .OrderByDescending(i => i.InvoiceSequence)
                    .Select(i => i.InvoiceSequence)
                    .FirstOrDefaultAsync() + 1);
        }
    }
}


