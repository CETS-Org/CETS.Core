using AutoMapper;
using Domain.Entities;
using DTOs.FIN_PaymentRefund.Requests;
using DTOs.FIN_PaymentRefund.Responses;

namespace Application.Mappers
{
	public class FIN_PaymentRefundProfile : Profile
	{
		public FIN_PaymentRefundProfile()
		{
			CreateMap<FIN_PaymentRefund, PaymentRefundResponse>().ReverseMap();
			CreateMap<CreatePaymentRefundRequest, FIN_PaymentRefund>();
			CreateMap<UpdatePaymentRefundRequest, FIN_PaymentRefund>();
		}
	}
}


