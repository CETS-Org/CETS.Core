using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.FIN_PaymentRefund.Requests
{
	public class UpdatePaymentRefundRequest
	{
		[Required]
		public Guid PaymentID { get; set; }

		public Guid? GatewayID { get; set; }

		[StringLength(255)]
		public string? RefundTxnId { get; set; }

		[Range(0, double.MaxValue)]
		public decimal Amount { get; set; }

		[StringLength(500)]
		public string? Reason { get; set; }

		[StringLength(30)]
		public string? GatewayStatus { get; set; }

		public string? GatewayPayload { get; set; }
	}
}


