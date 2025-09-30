using Domain.Entities;
using DTOs.FIN.FIN_InvoiceItem.Requests;
using DTOs.FIN.FIN_InvoiceItem.Responses;

namespace Application.Interfaces.FIN
{
	public interface IFIN_InvoiceItemService : IBaseService<FIN_InvoiceItem, InvoiceItemResponse, UpdateInvoiceItemRequest, CreateInvoiceItemRequest>
	{
		Task<IEnumerable<FIN_InvoiceItem>> GetByInvoiceIdAsync(Guid invoiceId);
	}
}


