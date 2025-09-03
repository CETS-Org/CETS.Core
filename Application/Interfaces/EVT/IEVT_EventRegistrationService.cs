using Domain.Entities;
using DTOs.EVT.EVT_EventRegistration.Requests;
using DTOs.EVT.EVT_EventRegistration.Responses;

namespace Application.Interfaces.EVT
{
	public interface IEVT_EventRegistrationService : IBaseService<EVT_EventRegistration, EventRegistrationResponse, UpdateEventRegistrationRequest, CreateEventRegistrationRequest>
	{
		Task<EventRegistrationResponse> CheckInAsync(Guid id, DateTime timestamp);
		Task<EventRegistrationResponse> CheckOutAsync(Guid id, DateTime timestamp);
		Task<IReadOnlyList<EventRegistrationResponse>> GetByEventIdAsync(Guid eventId);
		Task<IReadOnlyList<EventRegistrationResponse>> GetByAccountIdAsync(Guid accountId);
		Task<EventRegistrationResponse> SoftDeleteAsync(Guid id);
		Task<EventRegistrationResponse> RestoreRegistrationAsync(Guid id);
    }
}



