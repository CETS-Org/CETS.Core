using System;

namespace DTOs.FIN_PaymentRefund.Responses
{
	public class PaymentRefundResponse
	{
		public Guid Id { get; set; }
		public Guid PaymentID { get; set; }
		public Guid? GatewayID { get; set; }
		public string? RefundTxnId { get; set; }
		public decimal Amount { get; set; }
		public string? Reason { get; set; }
		public string? GatewayStatus { get; set; }
		public string? GatewayPayload { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public Guid? UpdatedBy { get; set; }
		public bool IsDeleted { get; set; }
	}
}


