using Domain.Entities;

namespace Domain.Interfaces.FIN
{
    public interface IFIN_PaymentWebhookRepository : IBaseRepository<FIN_PaymentWebhook>
    {
        Task<IEnumerable<FIN_PaymentWebhook>> GetAllWithDetailsAsync();
        Task<(IEnumerable<FIN_PaymentWebhook> Data, int TotalCount)> GetPaginatedAsync(
            string? eventType,
            string? accountName,
            DateTime? dateFrom,
            DateTime? dateTo,
            decimal? minAmount,
            decimal? maxAmount,
            int page,
            int pageSize);
    }
}


