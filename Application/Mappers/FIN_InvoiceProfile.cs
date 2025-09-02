using AutoMapper;
using Domain.Entities;
using DTOs.FIN_Invoice.Requests;
using DTOs.FIN_Invoice.Responses;

namespace Application.Mappers
{
	public class FIN_InvoiceProfile : Profile
	{
		public FIN_InvoiceProfile()
		{
			CreateMap<FIN_Invoice, InvoiceResponse>().ReverseMap();
			CreateMap<CreateInvoiceRequest, FIN_Invoice>();
			CreateMap<UpdateInvoiceRequest, FIN_Invoice>();
		}
	}
}


