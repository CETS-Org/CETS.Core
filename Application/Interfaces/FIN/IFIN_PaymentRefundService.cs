using Domain.Entities;
using DTOs.FIN_PaymentRefund.Requests;
using DTOs.FIN_PaymentRefund.Responses;

namespace Application.Interfaces.FIN
{
	public interface IFIN_PaymentRefundService : IBaseService<FIN_PaymentRefund, PaymentRefundResponse, UpdatePaymentRefundRequest, CreatePaymentRefundRequest>
	{
	}
}


