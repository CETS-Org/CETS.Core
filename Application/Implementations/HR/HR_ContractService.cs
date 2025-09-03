using Application.Interfaces.HR;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.HR;
using DTOs.HR.HR_Contract.Requests;
using DTOs.HR.HR_Contract.Responses;

namespace Application.Implementations.HR
{
	public class HR_ContractService : BaseService<HR_Contract, ContractResponse, UpdateContractRequest, CreateContractRequest>, IHR_ContractService
	{
		public HR_ContractService(IHR_ContractRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
			: base(repository, unitOfWork, mapper)
		{
		}

		public async Task<ContractResponse> SoftDeleteAsync(Guid id)
		{
			var entity = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"HR_Contract {id} not found.");
			entity.IsDeleted = true;
			_repository.Update(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<ContractResponse>(entity);
		}

		public async Task<ContractResponse> RestoreAsync(Guid id)
		{
			var entity = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"HR_Contract {id} not found.");
			entity.IsDeleted = false;
			_repository.Update(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<ContractResponse>(entity);
		}

		public async Task<IReadOnlyList<ContractResponse>> GetByTeacherIdAsync(Guid teacherId)
		{
			var items = await _repository.FindAsync(c => c.TeacherID == teacherId);
			return _mapper.Map<IReadOnlyList<ContractResponse>>(items);
		}
	}
}



