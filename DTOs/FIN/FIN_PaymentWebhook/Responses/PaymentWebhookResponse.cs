using System;

namespace DTOs.FIN.FIN_PaymentWebhook.Responses
{
	public class PaymentWebhookResponse
	{
		public Guid Id { get; set; }
		public Guid PaymentID { get; set; }
		public Guid EventId { get; set; }
		public Guid GatewayID { get; set; }
		public string EventType { get; set; } = null!;
		public DateTime ReceivedAt { get; set; }
		public string Payload { get; set; } = null!;
		public DateTime CreatedAt { get; set; }
		public bool IsDeleted { get; set; }
	}
}


