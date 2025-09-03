using AutoMapper;
using Domain.Entities;
using DTOs.EVT.EVT_Event.Requests;
using DTOs.EVT.EVT_Event.Responses;

namespace Application.Mappers
{
	public class EVT_EventProfile : Profile
	{
		public EVT_EventProfile()
		{
			CreateMap<EVT_Event, EventResponse>().ReverseMap();
			CreateMap<CreateEventRequest, EVT_Event>();
			CreateMap<UpdateEventRequest, EVT_Event>();
		}
	}
}



