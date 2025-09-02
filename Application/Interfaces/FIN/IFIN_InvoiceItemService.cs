using Domain.Entities;
using DTOs.FIN_InvoiceItem.Requests;
using DTOs.FIN_InvoiceItem.Responses;

namespace Application.Interfaces.FIN
{
	public interface IFIN_InvoiceItemService : IBaseService<FIN_InvoiceItem, InvoiceItemResponse, UpdateInvoiceItemRequest, CreateInvoiceItemRequest>
	{
	}
}


