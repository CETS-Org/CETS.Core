using Application.Interfaces.FIN;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.FIN;
using DTOs.FIN.FIN_Payment.Requests;
using DTOs.FIN.FIN_Payment.Responses;

namespace Application.Implementations.FIN
{
	public class FIN_PaymentService : BaseService<FIN_Payment, PaymentResponse, UpdatePaymentRequest, CreatePaymentRequest>, IFIN_PaymentService
	{
		public FIN_PaymentService(IFIN_PaymentRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
			: base(repository, unitOfWork, mapper)
		{
		}
	}
}


