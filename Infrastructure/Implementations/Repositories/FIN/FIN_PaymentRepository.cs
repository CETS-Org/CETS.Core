using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.FIN;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.FIN
{
    public class FIN_PaymentRepository : BaseRepository<FIN_Payment>, IFIN_PaymentRepository
    {
        public FIN_PaymentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<FIN_Payment>> GetPaymentsByStudentIdAsync(Guid studentId)
        {
            return await _context.FIN_Payments
                .Include(p => p.Invoice)
                    .ThenInclude(i => i.Student)
                        .ThenInclude(s => s.Account)
                .Include(p => p.Invoice)
                    .ThenInclude(i => i.InvoiceStatus)
                .Include(p => p.PaymentMethod)
                .Where(p => p.Invoice.StudentID == studentId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<FIN_Payment>> GetPaymentsByInvoiceIdAsync(Guid? invoiceId)
        {
            if (invoiceId == null)
                return Array.Empty<FIN_Payment>();

            return await _context.FIN_Payments
                .Include(p => p.Invoice)
                    .ThenInclude(i => i.Student)
                        .ThenInclude(s => s.Account)
                .Include(p => p.Invoice)
                    .ThenInclude(i => i.InvoiceStatus)
                .Include(p => p.PaymentMethod)
                .Where(p => p.InvoiceID == invoiceId) 
                .OrderBy(p => p.CreatedAt)             
                .ToListAsync();
        }



    }
}


