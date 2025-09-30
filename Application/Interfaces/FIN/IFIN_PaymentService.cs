using Domain.Entities;
using DTOs.FIN.FIN_Payment.Requests;
using DTOs.FIN.FIN_Payment.Responses;

namespace Application.Interfaces.FIN
{
	public interface IFIN_PaymentService : IBaseService<FIN_Payment, PaymentResponse, UpdatePaymentRequest, CreatePaymentRequest>
	{
		Task<FIN_Payment?> CreateMonthlyPayment(Guid invoiceId,Guid studentId, Guid reservationItemId);
	}
}


