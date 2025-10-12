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

		public override async Task<TeacherAvailabilityResponse> CreateAsync(CreateTeacherAvailabilityRequest createDto)
		{
			if (!createDto.TimeSlotID.HasValue)
			{
				throw new InvalidOperationException("TimeSlotID is required for teacher availability.");
			}

			var exists = await _repository.ExistsAsync(a =>
				a.TeacherID == createDto.TeacherID &&
				a.TeachDay == createDto.TeachDay &&
				a.TimeSlotID == createDto.TimeSlotID.Value);

			if (exists)
			{
				throw new InvalidOperationException("Teacher availability already exists for this teacher, day and time slot.");
			}

			return await base.CreateAsync(createDto);
		}

		public override async Task<TeacherAvailabilityResponse> UpdateAsync(Guid id, UpdateTeacherAvailabilityRequest dto)
		{
			if (!dto.TimeSlotID.HasValue)
			{
				throw new InvalidOperationException("TimeSlotID is required for teacher availability.");
			}

			var duplicateExists = await _repository.ExistsAsync(a =>
				a.Id != id &&
				a.TeacherID == dto.TeacherID &&
				a.TeachDay == dto.TeachDay &&
				a.TimeSlotID == dto.TimeSlotID.Value);

			if (duplicateExists)
			{
				throw new InvalidOperationException("Another availability already exists for this teacher, day and time slot.");
			}

			return await base.UpdateAsync(id, dto);
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



