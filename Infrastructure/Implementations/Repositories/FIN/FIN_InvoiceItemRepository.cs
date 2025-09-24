using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.FIN;
using Infrastructure.Implementations.Repositories;

namespace Infrastructure.Implementations.Repositories.FIN
{
    public class FIN_InvoiceItemRepository : BaseRepository<FIN_InvoiceItem>, IFIN_InvoiceItemRepository
    {
        public FIN_InvoiceItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}


