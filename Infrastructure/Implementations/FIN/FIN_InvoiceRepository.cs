using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.FIN;

namespace Infrastructure.Repositories.FIN
{
    public class FIN_InvoiceRepository : BaseRepository<FIN_Invoice>, IFIN_InvoiceRepository
    {
        public FIN_InvoiceRepository(AppDbContext context) : base(context)
        {
        }
    }
}


