using System.Collections.Generic;

namespace DTOs.FIN.FIN_PaymentWebhook.Responses
{
	public class PaginatedPaymentWebhookResponse
	{
		public List<PaymentWebhookResponse> Data { get; set; } = new();
		public int TotalCount { get; set; }
		public int Page { get; set; }
		public int PageSize { get; set; }
		public int TotalPages { get; set; }
	}
}
