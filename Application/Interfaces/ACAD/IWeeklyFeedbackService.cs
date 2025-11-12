using DTOs.ACAD.ACAD_WeeklyFeedback.Request;
using DTOs.ACAD.ACAD_WeeklyFeedback.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IWeeklyFeedbackService
    {
        Task UpsertAsync(Guid teacherId, UpsertWeeklyFeedbackRequestDto req, CancellationToken ct = default);

        Task<IReadOnlyList<WeeklyFeedbackViewDto>> GetByClassWeekAsync(
            Guid ClassID, int weekNumber, CancellationToken ct = default);

        Task<IReadOnlyList<WeeklyFeedbackViewDto>> GetByStudentAsync(
            Guid studentId, Guid? ClassID, CancellationToken ct = default);

        Task<WeeklyFeedbackViewDto?> GetDetailAsync(Guid id, CancellationToken ct = default);
    }
}
