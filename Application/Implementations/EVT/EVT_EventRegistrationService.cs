using Application.Interfaces.EVT;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.EVT;
using DTOs.EVT.EVT_EventRegistration.Requests;
using DTOs.EVT.EVT_EventRegistration.Responses;

namespace Application.Implementations.EVT
{
	public class EVT_EventRegistrationService : BaseService<EVT_EventRegistration, EventRegistrationResponse, UpdateEventRegistrationRequest, CreateEventRegistrationRequest>, IEVT_EventRegistrationService
	{
		public EVT_EventRegistrationService(IEVT_EventRegistrationRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
			: base(repository, unitOfWork, mapper)
		{
		}

		public async Task<EventRegistrationResponse> CheckInAsync(Guid id, DateTime timestamp)
		{
			var entity = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Registration {id} not found.");
			entity.CheckInAt = timestamp;
			_repository.Update(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<EventRegistrationResponse>(entity);
		}

		public async Task<EventRegistrationResponse> CheckOutAsync(Guid id, DateTime timestamp)
		{
			var entity = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Registration {id} not found.");
			entity.CheckOutAt = timestamp;
			_repository.Update(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<EventRegistrationResponse>(entity);
		}

		public async Task<IReadOnlyList<EventRegistrationResponse>> GetByEventIdAsync(Guid eventId)
		{
			var items = await _repository.FindAsync(r => r.EventID == eventId && !r.IsDeleted);
			return _mapper.Map<IReadOnlyList<EventRegistrationResponse>>(items);
		}

		public async Task<IReadOnlyList<EventRegistrationResponse>> GetByAccountIdAsync(Guid accountId)
		{
			var items = await _repository.FindAsync(r => r.AccountID == accountId && !r.IsDeleted);
			return _mapper.Map<IReadOnlyList<EventRegistrationResponse>>(items);
		}

		public async Task<EventRegistrationResponse> SoftDeleteAsync(Guid id)
		{
			var entity = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Registration {id} not found.");
			entity.IsDeleted = true;
			_repository.Update(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<EventRegistrationResponse>(entity);
        }

		public async Task<EventRegistrationResponse> RestoreRegistrationAsync(Guid id)
		{
			var entity = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Registration {id} not found.");
			entity.IsDeleted = false;
			_repository.Update(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<EventRegistrationResponse>(entity);
        }
    }
}



