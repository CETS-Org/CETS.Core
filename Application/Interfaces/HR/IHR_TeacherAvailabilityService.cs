using Domain.Entities;
using DTOs.HR.HR_TeacherAvailability.Requests;
using DTOs.HR.HR_TeacherAvailability.Responses;

namespace Application.Interfaces.HR
{
	public interface IHR_TeacherAvailabilityService : IBaseService<HR_TeacherAvailability, TeacherAvailabilityResponse, UpdateTeacherAvailabilityRequest, CreateTeacherAvailabilityRequest>
	{
		Task<IReadOnlyList<TeacherAvailabilityResponse>> GetByTeacherIdAsync(Guid teacherId);
		Task<IReadOnlyList<TeacherAvailabilityResponse>> GetByTeacherAndDateAsync(Guid teacherId, DateTime teachDate);
	}
}



