using Application.Interfaces.EVT;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.EVT;
using DTOs.EVT.EVT_EventFeedback.Requests;
using DTOs.EVT.EVT_EventFeedback.Responses;

namespace Application.Implementations.EVT
{
	public class EVT_EventFeedbackService : BaseService<EVT_EventFeedback, EventFeedbackResponse, UpdateEventFeedbackRequest, CreateEventFeedbackRequest>, IEVT_EventFeedbackService
	{
		public EVT_EventFeedbackService(IEVT_EventFeedbackRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
			: base(repository, unitOfWork, mapper)
		{
		}

		public async Task<IReadOnlyList<EventFeedbackResponse>> GetByEventIdAsync(Guid eventId)
		{
			var items = await _repository.FindAsync(f => f.EventID == eventId);
			return _mapper.Map<IReadOnlyList<EventFeedbackResponse>>(items);
		}
	}
}



