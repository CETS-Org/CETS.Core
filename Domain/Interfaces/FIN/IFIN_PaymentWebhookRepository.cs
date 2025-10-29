using Domain.Entities;

namespace Domain.Interfaces.FIN
{
    public interface IFIN_PaymentWebhookRepository : IBaseRepository<FIN_PaymentWebhook>
    {
        Task<IEnumerable<FIN_PaymentWebhook>> GetAllWithDetailsAsync();
    }
}


