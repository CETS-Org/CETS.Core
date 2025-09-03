using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.FIN.FIN_PaymentWebhook.Requests
{
	public class UpdatePaymentWebhookRequest
	{
		[Required]
		public Guid PaymentID { get; set; }

		[Required]
		public Guid EventId { get; set; }

		[Required]
		public Guid GatewayID { get; set; }

		[Required]
		[StringLength(100)]
		public string EventType { get; set; } = null!;

		[Required]
		public DateTime ReceivedAt { get; set; }

		[Required]
		public string Payload { get; set; } = null!;
	}
}


