using AutoMapper;
using Domain.Entities;
using DTOs.EVT.EVT_EventRegistration.Requests;
using DTOs.EVT.EVT_EventRegistration.Responses;

namespace Application.Mappers
{
	public class EVT_EventRegistrationProfile : Profile
	{
		public EVT_EventRegistrationProfile()
		{
			CreateMap<EVT_EventRegistration, EventRegistrationResponse>().ReverseMap();
			CreateMap<CreateEventRegistrationRequest, EVT_EventRegistration>();
			CreateMap<UpdateEventRegistrationRequest, EVT_EventRegistration>();
		}
	}
}



