using Application.Interfaces.HR;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.HR;
using DTOs.HR.HR_TeacherAvailability.Requests;
using DTOs.HR.HR_TeacherAvailability.Responses;

namespace Application.Implementations.HR
{
	public class HR_TeacherAvailabilityService : BaseService<HR_TeacherAvailability, TeacherAvailabilityResponse, UpdateTeacherAvailabilityRequest, CreateTeacherAvailabilityRequest>, IHR_TeacherAvailabilityService
	{
		public HR_TeacherAvailabilityService(IHR_TeacherAvailabilityRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
			: base(repository, unitOfWork, mapper)
		{
		}

		public async Task<IReadOnlyList<TeacherAvailabilityResponse>> GetByTeacherIdAsync(Guid teacherId)
		{
			var items = await _repository.FindAsync(a => a.TeacherID == teacherId);
			return _mapper.Map<IReadOnlyList<TeacherAvailabilityResponse>>(items);
		}

		public async Task<IReadOnlyList<TeacherAvailabilityResponse>> GetByTeacherAndDateAsync(Guid teacherId, DayOfWeek teachDay)
		{
			var items = await _repository.FindAsync(a => a.TeacherID == teacherId && a.TeachDay == teachDay);
			return _mapper.Map<IReadOnlyList<TeacherAvailabilityResponse>>(items);
		}
	}
}



