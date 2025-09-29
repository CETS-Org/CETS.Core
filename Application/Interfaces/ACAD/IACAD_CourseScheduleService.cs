using Domain.Entities;
using DTOs.ACAD.ACAD_CourseSchedule.Requests;
using DTOs.ACAD.ACAD_CourseSchedule.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_CourseScheduleService : IBaseService<ACAD_CourseSchedule, CourseScheduleResponse, UpdateCourseScheduleRequest, CreateCourseScheduleRequest>
    {
        Task<IEnumerable<CourseScheduleResponse>> GetSchedulesByCourseIdAsync(Guid courseId);
        Task<IEnumerable<CourseScheduleResponse>> GetSchedulesByDayOfWeekAsync(string dayOfWeek);
        Task<IEnumerable<CourseScheduleResponse>> GetSchedulesByTimeSlotIdAsync(Guid timeSlotId);
        Task<bool> IsTimeSlotAvailableAsync(Guid courseId, Guid timeSlotId, string dayOfWeek);
        Task<CourseScheduleResponse?> GetDetailByIdAsync(Guid id);
    }
}
