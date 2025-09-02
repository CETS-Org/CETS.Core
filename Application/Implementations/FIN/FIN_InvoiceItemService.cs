using Application.Interfaces.FIN;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.FIN;
using DTOs.FIN_InvoiceItem.Requests;
using DTOs.FIN_InvoiceItem.Responses;

namespace Application.Implementations.FIN
{
	public class FIN_InvoiceItemService : BaseService<FIN_InvoiceItem, InvoiceItemResponse, UpdateInvoiceItemRequest, CreateInvoiceItemRequest>, IFIN_InvoiceItemService
	{
		public FIN_InvoiceItemService(IFIN_InvoiceItemRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
			: base(repository, unitOfWork, mapper)
		{
		}
	}
}


