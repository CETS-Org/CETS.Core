using AutoMapper;
using Domain.Entities;
using DTOs.FIN_Payment.Requests;
using DTOs.FIN_Payment.Responses;

namespace Application.Mappers
{
	public class FIN_PaymentProfile : Profile
	{
		public FIN_PaymentProfile()
		{
			CreateMap<FIN_Payment, PaymentResponse>().ReverseMap();
			CreateMap<CreatePaymentRequest, FIN_Payment>();
			CreateMap<UpdatePaymentRequest, FIN_Payment>();
		}
	}
}


