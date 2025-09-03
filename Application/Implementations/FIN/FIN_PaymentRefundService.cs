using Application.Interfaces.FIN;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.FIN;
using DTOs.FIN.FIN_PaymentRefund.Requests;
using DTOs.FIN.FIN_PaymentRefund.Responses;

namespace Application.Implementations.FIN
{
	public class FIN_PaymentRefundService : BaseService<FIN_PaymentRefund, PaymentRefundResponse, UpdatePaymentRefundRequest, CreatePaymentRefundRequest>, IFIN_PaymentRefundService
	{
		public FIN_PaymentRefundService(IFIN_PaymentRefundRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
			: base(repository, unitOfWork, mapper)
		{
		}
	}
}


