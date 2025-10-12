using Application.Interfaces.HR;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.HR;
using DTOs.HR.HR_TeacherAvailability.Requests;
using DTOs.HR.HR_TeacherAvailability.Responses;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Implementations.HR
{
	public class HR_TeacherAvailabilityService : BaseService<HR_TeacherAvailability, TeacherAvailabilityResponse, UpdateTeacherAvailabilityRequest, CreateTeacherAvailabilityRequest>, IHR_TeacherAvailabilityService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public HR_TeacherAvailabilityService(IHR_TeacherAvailabilityRepository repository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
			: base(repository, unitOfWork, mapper)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		private bool IsTeacherValidRole()
		{
			var user = _httpContextAccessor.HttpContext?.User;
			if (user == null) return false;
			bool isTeacher = user.IsInRole("Teacher");
			bool isPrivileged = user.IsInRole("Admin") || user.IsInRole("AcademicStaff");
			return isTeacher && !isPrivileged;
		}

		private bool IsInsideModificationPeriod()
		{
            if (!IsTeacherValidRole() && DateTime.Now.Day > 10)
            {
				return false;
            }

			return true;
        }

        public override async Task<TeacherAvailabilityResponse> CreateAsync(CreateTeacherAvailabilityRequest createDto)
		{
			if (!IsInsideModificationPeriod())	
			{
				throw new InvalidOperationException("Teachers can modify availability only until the 10th day of the month.");
            }

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
            if (!IsInsideModificationPeriod())
            {
                throw new InvalidOperationException("Teachers can modify availability only until the 10th day of the month.");
            }
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

		public override async Task DeleteAsync(Guid id)
		{
            if (!IsInsideModificationPeriod())
            {
                throw new InvalidOperationException("Teachers can modify availability only until the 10th day of the month.");
            }
            await base.DeleteAsync(id);
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



