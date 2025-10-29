using Application.Interfaces.FIN;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.FIN;
using DTOs.FIN.FIN_PaymentWebhook.Requests;
using DTOs.FIN.FIN_PaymentWebhook.Responses;

namespace Application.Implementations.FIN
{
	public class FIN_PaymentWebhookService : BaseService<FIN_PaymentWebhook, PaymentWebhookResponse, UpdatePaymentWebhookRequest, CreatePaymentWebhookRequest>, IFIN_PaymentWebhookService
	{
		private readonly IFIN_PaymentWebhookRepository _webhookRepository;

		public FIN_PaymentWebhookService(IFIN_PaymentWebhookRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
			: base(repository, unitOfWork, mapper)
		{
			_webhookRepository = repository;
		}

		public override async Task<IReadOnlyList<PaymentWebhookResponse>> GetAllAsync()
		{
			var webhooks = await _webhookRepository.GetAllWithDetailsAsync();

			var responses = webhooks.Select(webhook => new PaymentWebhookResponse
			{
				Id = webhook.Id,
				PaymentID = webhook.PaymentID,
				EventId = webhook.EventId,
				GatewayID = webhook.GatewayID,
				EventType = webhook.EventType,
				ReceivedAt = webhook.ReceivedAt,
				Payload = webhook.Payload,
				CreatedAt = webhook.CreatedAt,

				// Additional fields from related entities
				CreatedByName = webhook.Payment?.Invoice?.CreatedByNavigation?.FullName,
				PaymentAmount = webhook.Payment?.Amount,
				CourseName = webhook.Payment?.Invoice?.FIN_InvoiceItems?.FirstOrDefault()?.Course?.CourseName,
				GatewayName = webhook.Gateway?.Name
			}).ToList();

			return responses;
		}
	}
}


