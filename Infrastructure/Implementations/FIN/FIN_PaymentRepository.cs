using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.FIN;

namespace Infrastructure.Repositories.FIN
{
    public class FIN_PaymentRepository : BaseRepository<FIN_Payment>, IFIN_PaymentRepository
    {
        public FIN_PaymentRepository(AppDbContext context) : base(context)
        {
        }
    }
}


