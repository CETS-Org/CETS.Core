using AutoMapper;
using Domain.Entities;
using DTOs.FIN.FIN_InvoiceItem.Requests;
using DTOs.FIN.FIN_InvoiceItem.Responses;

namespace Application.Mappers.FIN
{
	public class FIN_InvoiceItemProfile : Profile
	{
		public FIN_InvoiceItemProfile()
		{
			CreateMap<FIN_InvoiceItem, InvoiceItemResponse>().ReverseMap();
			CreateMap<CreateInvoiceItemRequest, FIN_InvoiceItem>();
			CreateMap<UpdateInvoiceItemRequest, FIN_InvoiceItem>();
		}
	}
}


