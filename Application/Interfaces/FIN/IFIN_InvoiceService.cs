using Domain.Entities;
using DTOs.FIN.FIN_Invoice.Requests;
using DTOs.FIN.FIN_Invoice.Responses;

namespace Application.Interfaces.FIN
{
	public interface IFIN_InvoiceService : IBaseService<FIN_Invoice, InvoiceResponse, UpdateInvoiceRequest, CreateInvoiceRequest>
	{
		Task<FIN_Invoice?> CreateInvolcesTopay(Guid reservationId,Guid studentId);
    }
}


