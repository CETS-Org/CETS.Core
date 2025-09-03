using AutoMapper;
using Domain.Entities;
using DTOs.HR.HR_Contract.Requests;
using DTOs.HR.HR_Contract.Responses;

namespace Application.Mappers
{
	public class HR_ContractProfile : Profile
	{
		public HR_ContractProfile()
		{
			CreateMap<HR_Contract, ContractResponse>().ReverseMap();
			CreateMap<CreateContractRequest, HR_Contract>();
			CreateMap<UpdateContractRequest, HR_Contract>();
		}
	}
}



