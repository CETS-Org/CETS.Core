using System;

namespace DTOs.FIN.FIN_PaymentWebhook.Requests
{
	public class GetPaginatedPaymentWebhookRequest
	{
		public string? EventType { get; set; }
		public string? AccountName { get; set; }
		public DateTime? DateFrom { get; set; }
		public DateTime? DateTo { get; set; }
		public decimal? MinAmount { get; set; }
		public decimal? MaxAmount { get; set; }
		public int Page { get; set; } = 1;
		public int PageSize { get; set; } = 20;
	}
}
