using Domain.Entities;
using DTOs.EVT.EVT_EventFeedback.Requests;
using DTOs.EVT.EVT_EventFeedback.Responses;

namespace Application.Interfaces.EVT
{
	public interface IEVT_EventFeedbackService : IBaseService<EVT_EventFeedback, EventFeedbackResponse, UpdateEventFeedbackRequest, CreateEventFeedbackRequest>
	{
		Task<IReadOnlyList<EventFeedbackResponse>> GetByEventIdAsync(Guid eventId);
	}
}



