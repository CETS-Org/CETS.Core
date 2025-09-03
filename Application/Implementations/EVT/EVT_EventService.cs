using Application.Interfaces.EVT;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.EVT;
using DTOs.EVT.EVT_Event.Requests;
using DTOs.EVT.EVT_Event.Responses;

namespace Application.Implementations.EVT
{
	public class EVT_EventService : BaseService<EVT_Event, EventResponse, UpdateEventRequest, CreateEventRequest>, IEVT_EventService
	{
		private readonly IEVT_EventRepository _repositoryTyped;

		public EVT_EventService(IEVT_EventRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
			: base(repository, unitOfWork, mapper)
		{
			_repositoryTyped = repository;
		}

		public async Task<IReadOnlyList<EventResponse>> GetByTypeIdAsync(Guid eventTypeId)
		{
			var items = await _repository.FindAsync(e => e.EventTypeID == eventTypeId && !e.IsDeleted);
			return _mapper.Map<IReadOnlyList<EventResponse>>(items);
		}

		public async Task<EventResponse> SoftDeleteAsync(Guid id)
		{
			var entity = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Event {id} not found.");
			entity.IsDeleted = true;
			_repository.Update(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<EventResponse>(entity);
        }

		public async Task<EventResponse> RestoreEventAsync(Guid id)
		{
			var entity = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Event {id} not found.");
			entity.IsDeleted = false;
			_repository.Update(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<EventResponse>(entity);
        }
    }
}



