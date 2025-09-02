using Application.Interfaces.FIN;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.FIN;
using DTOs.FIN_PaymentWebhook.Requests;
using DTOs.FIN_PaymentWebhook.Responses;

namespace Application.Implementations.FIN
{
	public class FIN_PaymentWebhookService : BaseService<FIN_PaymentWebhook, PaymentWebhookResponse, UpdatePaymentWebhookRequest, CreatePaymentWebhookRequest>, IFIN_PaymentWebhookService
	{
		public FIN_PaymentWebhookService(IFIN_PaymentWebhookRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
			: base(repository, unitOfWork, mapper)
		{
		}
	}
}


