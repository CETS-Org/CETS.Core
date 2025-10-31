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

		// Additional fields from related entities
		public string? CreatedByName { get; set; }
		public decimal? PaymentAmount { get; set; }
		public string? CourseName { get; set; }
		public string? GatewayName { get; set; }
	}
}


