using Domain.Entities;

namespace Domain.Interfaces.FIN
{
    public interface IFIN_InvoiceRepository : IBaseRepository<FIN_Invoice>
    {
        Task<int> GetNextSequenceInvoiceIdAsync();
        Task<IEnumerable<FIN_Invoice>> GetUnpaidInvoicesByStudentAsync(Guid studentId);
    }
}


