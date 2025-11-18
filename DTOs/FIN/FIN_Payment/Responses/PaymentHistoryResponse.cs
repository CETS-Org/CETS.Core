using System;

namespace DTOs.FIN.FIN_Payment.Responses
{
    public class PaymentHistoryResponse
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public Guid InvoiceId { get; set; }
        public string InvoiceStatus { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public InstallmentInfoResponse? InstallmentInfo { get; set; }
    }

    public class InstallmentInfoResponse
    {
        public int CurrentInstallment { get; set; }
        public DateTime? NextDueDate { get; set; }
    }
}
