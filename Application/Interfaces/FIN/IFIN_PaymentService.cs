using Domain.Entities;
using DTOs.FIN_Payment.Requests;
using DTOs.FIN_Payment.Responses;

namespace Application.Interfaces.FIN
{
	public interface IFIN_PaymentService : IBaseService<FIN_Payment, PaymentResponse, UpdatePaymentRequest, CreatePaymentRequest>
	{
	}
}


