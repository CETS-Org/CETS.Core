using Domain.Entities;
using DTOs.EVT.EVT_Event.Requests;
using DTOs.EVT.EVT_Event.Responses;
using DTOs.EVT.EVT_EventRegistration.Responses;

namespace Application.Interfaces.EVT
{
	public interface IEVT_EventService : IBaseService<EVT_Event, EventResponse, UpdateEventRequest, CreateEventRequest>
	{
		Task<IReadOnlyList<EventResponse>> GetByTypeIdAsync(Guid eventTypeId);
		Task<EventResponse> SoftDeleteAsync(Guid id);
        Task<EventResponse> RestoreEventAsync(Guid id);

    }
}



