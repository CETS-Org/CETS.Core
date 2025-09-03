using Domain.Entities;
using DTOs.HR.HR_Contract.Requests;
using DTOs.HR.HR_Contract.Responses;

namespace Application.Interfaces.HR
{
	public interface IHR_ContractService : IBaseService<HR_Contract, ContractResponse, UpdateContractRequest, CreateContractRequest>
	{
		Task<ContractResponse> SoftDeleteAsync(Guid id);
		Task<ContractResponse> RestoreAsync(Guid id);
		Task<IReadOnlyList<ContractResponse>> GetByTeacherIdAsync(Guid teacherId);
	}
}



