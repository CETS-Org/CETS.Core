namespace DTOs.FIN.FIN_Payment.Responses
{
    public class PaymentReturnResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public string? OrderCode { get; set; }
        public string? Status { get; set; }
        public decimal? Amount { get; set; }
        public Guid? InvoiceId { get; set; }
        public string? RedirectUrl { get; set; }
    }
}

