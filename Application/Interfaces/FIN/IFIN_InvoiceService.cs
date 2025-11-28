using Domain.Entities;
using DTOs.FIN.FIN_Invoice.Requests;
using DTOs.FIN.FIN_Invoice.Responses;

namespace Application.Interfaces.FIN
{
	public interface IFIN_InvoiceService : IBaseService<FIN_Invoice, InvoiceResponse, UpdateInvoiceRequest, CreateInvoiceRequest>
	{
		Task<InvoiceResponse?> CreateInvolcesToMonthlyPay(Guid reservationId,Guid studentId, decimal amount);
		Task<InvoiceResponse?> CreateInvoiceForFullPayment(Guid reservationId, Guid studentId, decimal amount);
		Task<FIN_Invoice> updateInvoiceStatus(Guid invoiceId, Guid statusId);
    }
}


