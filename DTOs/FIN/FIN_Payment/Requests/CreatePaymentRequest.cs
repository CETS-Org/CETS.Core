using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.FIN.FIN_Payment.Requests
{
	public class CreatePaymentRequest
	{
		[Required]
		public Guid InvoiceID { get; set; }

		[Required]
		public Guid PaymentMethodID { get; set; }

		public Guid? GatewayID { get; set; }

		[StringLength(255)]
		public string? TransactionID { get; set; }

		[Range(0, double.MaxValue)]
		public decimal Amount { get; set; }

		[Required]
		public DateTime PaymentDate { get; set; }

		public string? GatewayStatus { get; set; }
		public string? GatewayPayload { get; set; }
	}
}


