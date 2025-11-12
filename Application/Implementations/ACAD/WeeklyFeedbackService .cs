using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_WeeklyFeedback.Request;
using DTOs.ACAD.ACAD_WeeklyFeedback.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.ACAD
{
    public class WeeklyFeedbackService  : IWeeklyFeedbackService
    {
        private readonly IWeeklyFeedbackRepository _weeklyFeedbackRepository;
       

        public WeeklyFeedbackService(IWeeklyFeedbackRepository weeklyFeedbackRepository)
        {
            _weeklyFeedbackRepository = weeklyFeedbackRepository;          
        }

        public async Task UpsertAsync(Guid teacherId, UpsertWeeklyFeedbackRequestDto req, CancellationToken ct = default)
        {
            await _weeklyFeedbackRepository.UpsertAsync(teacherId, req, ct);
        }


        public async Task<IReadOnlyList<WeeklyFeedbackViewDto>> GetByClassWeekAsync(
            Guid ClassID, int weekNumber, CancellationToken ct = default)
        { 
            return await _weeklyFeedbackRepository.GetByClassWeekAsync(ClassID, weekNumber, ct); 
        }

        public async Task<IReadOnlyList<WeeklyFeedbackViewDto>> GetByStudentAsync(
            Guid studentId, Guid? ClassID, CancellationToken ct = default)
        {
            return await _weeklyFeedbackRepository.GetByStudentAsync(studentId, ClassID, ct);
        }

        public async Task<WeeklyFeedbackViewDto?> GetDetailAsync(Guid id, CancellationToken ct = default)
        {
            return await _weeklyFeedbackRepository.GetDetailAsync(id, ct);
        }


    }
}
