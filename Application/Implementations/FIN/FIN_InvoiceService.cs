using Application.Interfaces.FIN;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.FIN;
using DTOs.FIN.FIN_Invoice.Requests;
using DTOs.FIN.FIN_Invoice.Responses;

namespace Application.Implementations.FIN
{
	public class FIN_InvoiceService : BaseService<FIN_Invoice, InvoiceResponse, UpdateInvoiceRequest, CreateInvoiceRequest>, IFIN_InvoiceService
	{
		public FIN_InvoiceService(IFIN_InvoiceRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
			: base(repository, unitOfWork, mapper)
		{
		}
	}
}


