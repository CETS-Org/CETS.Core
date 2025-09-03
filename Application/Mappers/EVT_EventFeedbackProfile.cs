using AutoMapper;
using Domain.Entities;
using DTOs.EVT.EVT_EventFeedback.Requests;
using DTOs.EVT.EVT_EventFeedback.Responses;

namespace Application.Mappers
{
	public class EVT_EventFeedbackProfile : Profile
	{
		public EVT_EventFeedbackProfile()
		{
			CreateMap<EVT_EventFeedback, EventFeedbackResponse>().ReverseMap();
			CreateMap<CreateEventFeedbackRequest, EVT_EventFeedback>();
			CreateMap<UpdateEventFeedbackRequest, EVT_EventFeedback>();
		}
	}
}



