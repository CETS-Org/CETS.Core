using AutoMapper;
using Domain.Entities;
using DTOs.HR.HR_TeacherAvailability.Requests;
using DTOs.HR.HR_TeacherAvailability.Responses;

namespace Application.Mappers.HR
{
	public class HR_TeacherAvailabilityProfile : Profile
	{
		public HR_TeacherAvailabilityProfile()
		{
			CreateMap<HR_TeacherAvailability, TeacherAvailabilityResponse>()
				.ForMember(dest => dest.TeachDay, opt => opt.MapFrom(src => src.TeachDay.ToString()))
                .ReverseMap();

			CreateMap<CreateTeacherAvailabilityRequest, HR_TeacherAvailability>();
			CreateMap<UpdateTeacherAvailabilityRequest, HR_TeacherAvailability>();
		}
	}
}



