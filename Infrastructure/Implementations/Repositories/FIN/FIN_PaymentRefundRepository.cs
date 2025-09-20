using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.FIN;
using Infrastructure.Implementations.Repositories;

namespace Infrastructure.Implementations.Repositories.FIN
{
    public class FIN_PaymentRefundRepository : BaseRepository<FIN_PaymentRefund>, IFIN_PaymentRefundRepository
    {
        public FIN_PaymentRefundRepository(AppDbContext context) : base(context)
        {
        }
    }
}


