using Domain.Entities;
using DTOs.FIN_PaymentWebhook.Requests;
using DTOs.FIN_PaymentWebhook.Responses;

namespace Application.Interfaces.FIN
{
	public interface IFIN_PaymentWebhookService : IBaseService<FIN_PaymentWebhook, PaymentWebhookResponse, UpdatePaymentWebhookRequest, CreatePaymentWebhookRequest>
	{
	}
}


