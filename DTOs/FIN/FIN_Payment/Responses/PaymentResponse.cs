using System;

namespace DTOs.FIN.FIN_Payment.Responses
{
	public class PaymentResponse
	{
		public Guid Id { get; set; }
		public Guid InvoiceID { get; set; }
		public Guid PaymentMethodID { get; set; }
		public Guid? GatewayID { get; set; }
		public string? GatewayStatus { get; set; }
		public string? TransactionID { get; set; }
		public decimal Amount { get; set; }
		public DateTime PaymentDate { get; set; }
		public string? GatewayPayload { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public Guid? UpdatedBy { get; set; }
		public bool IsDeleted { get; set; }		
    }
}


