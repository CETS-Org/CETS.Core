using Domain.Entities;

namespace Domain.Interfaces.FIN
{
    public interface IFIN_PaymentRepository : IBaseRepository<FIN_Payment>
    {
        Task<IReadOnlyList<FIN_Payment>> GetPaymentsByStudentIdAsync(Guid studentId);
    }
}


