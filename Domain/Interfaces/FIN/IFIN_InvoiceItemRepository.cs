using Domain.Entities;

namespace Domain.Interfaces.FIN
{
    public interface IFIN_InvoiceItemRepository : IBaseRepository<FIN_InvoiceItem>
    {
        Task<IEnumerable<FIN_InvoiceItem>> GetByInvoiceIdAsync(Guid invoiceId);
        Task<List<FIN_InvoiceItem>> GetByInvoiceIdsAsync(List<Guid> invoiceIds);
    }

}


