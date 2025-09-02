using Domain.Entities;
using DTOs.FIN_Invoice.Requests;
using DTOs.FIN_Invoice.Responses;

namespace Application.Interfaces.FIN
{
	public interface IFIN_InvoiceService : IBaseService<FIN_Invoice, InvoiceResponse, UpdateInvoiceRequest, CreateInvoiceRequest>
	{
	}
}


