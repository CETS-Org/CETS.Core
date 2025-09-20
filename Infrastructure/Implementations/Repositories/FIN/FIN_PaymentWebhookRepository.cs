using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.FIN;
using Infrastructure.Implementations.Repositories;

namespace Infrastructure.Implementations.Repositories.FIN
{
    public class FIN_PaymentWebhookRepository : BaseRepository<FIN_PaymentWebhook>, IFIN_PaymentWebhookRepository
    {
        public FIN_PaymentWebhookRepository(AppDbContext context) : base(context)
        {
        }
    }
}


