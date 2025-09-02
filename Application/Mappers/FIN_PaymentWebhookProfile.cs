using AutoMapper;
using Domain.Entities;
using DTOs.FIN_PaymentWebhook.Requests;
using DTOs.FIN_PaymentWebhook.Responses;

namespace Application.Mappers
{
	public class FIN_PaymentWebhookProfile : Profile
	{
		public FIN_PaymentWebhookProfile()
		{
			CreateMap<FIN_PaymentWebhook, PaymentWebhookResponse>().ReverseMap();
			CreateMap<CreatePaymentWebhookRequest, FIN_PaymentWebhook>();
			CreateMap<UpdatePaymentWebhookRequest, FIN_PaymentWebhook>();
		}
	}
}


