using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_CourseSchedule.Requests;
using DTOs.ACAD.ACAD_CourseSchedule.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.ACAD
{
    public class ACAD_CourseScheduleService : BaseService<ACAD_CourseSchedule, CourseScheduleResponse, UpdateCourseScheduleRequest, CreateCourseScheduleRequest>, IACAD_CourseScheduleService
    {
        private readonly IACAD_CourseScheduleRepository _courseScheduleRepository;

        public ACAD_CourseScheduleService(
            IACAD_CourseScheduleRepository courseScheduleRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper) : base(courseScheduleRepository, unitOfWork, mapper)
        {
            _courseScheduleRepository = courseScheduleRepository;
        }

        public async Task<IEnumerable<CourseScheduleResponse>> GetSchedulesByCourseIdAsync(Guid courseId)
        {
            var schedules = await _courseScheduleRepository.GetSchedulesByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<CourseScheduleResponse>>(schedules);
        }

        public async Task<IEnumerable<CourseScheduleResponse>> GetSchedulesByDayOfWeekAsync(string dayOfWeek)
        {
            var schedules = await _courseScheduleRepository.GetSchedulesByDayOfWeekAsync(dayOfWeek);
            return _mapper.Map<IEnumerable<CourseScheduleResponse>>(schedules);
        }

        public async Task<IEnumerable<CourseScheduleResponse>> GetSchedulesByTimeSlotIdAsync(Guid timeSlotId)
        {
            var schedules = await _courseScheduleRepository.GetSchedulesByTimeSlotIdAsync(timeSlotId);
            return _mapper.Map<IEnumerable<CourseScheduleResponse>>(schedules);
        }

        public async Task<bool> IsTimeSlotAvailableAsync(Guid courseId, Guid timeSlotId, string dayOfWeek)
        {
            return await _courseScheduleRepository.IsTimeSlotAvailableAsync(courseId, timeSlotId, dayOfWeek);
        }

        public async Task<CourseScheduleResponse?> GetDetailByIdAsync(Guid id)
        {
            var schedule = await _courseScheduleRepository.GetDetailByIdAsync(id);
            return _mapper.Map<CourseScheduleResponse?>(schedule);
        }

        public override async Task<IReadOnlyList<CourseScheduleResponse>> GetAllAsync()
        {
            var schedules = await _courseScheduleRepository.GetAllWithNavigationPropertiesAsync();
            return _mapper.Map<IReadOnlyList<CourseScheduleResponse>>(schedules);
        }

        public override async Task<CourseScheduleResponse?> GetByIdAsync(Guid id)
        {
            var schedule = await _courseScheduleRepository.GetDetailByIdAsync(id);
            return _mapper.Map<CourseScheduleResponse?>(schedule);
        }

        public override async Task<CourseScheduleResponse> CreateAsync(CreateCourseScheduleRequest createDto)
        {
            // Check if the time slot is available
            var isAvailable = await _courseScheduleRepository.IsTimeSlotAvailableAsync(
                createDto.CourseID, 
                createDto.TimeSlotID, 
                createDto.DayOfWeek);

            if (!isAvailable)
            {
                throw new InvalidOperationException("This time slot is already taken for the specified course and day.");
            }

            return await base.CreateAsync(createDto);
        }

        public override async Task<CourseScheduleResponse> UpdateAsync(Guid id, UpdateCourseScheduleRequest dto)
        {
            var existingSchedule = await _courseScheduleRepository.GetByIdAsync(id);
            if (existingSchedule == null)
            {
                throw new KeyNotFoundException($"Course schedule with id {id} not found.");
            }

            // Check if the time slot is available (excluding the current schedule)
            var isAvailable = await _courseScheduleRepository.IsTimeSlotAvailableAsync(
                dto.CourseID, 
                dto.TimeSlotID, 
                dto.DayOfWeek);

            if (!isAvailable && (existingSchedule.CourseID != dto.CourseID || 
                                existingSchedule.TimeSlotID != dto.TimeSlotID || 
                                existingSchedule.DayOfWeek != dto.DayOfWeek))
            {
                throw new InvalidOperationException("This time slot is already taken for the specified course and day.");
            }

            return await base.UpdateAsync(id, dto);
        }
    }
}
