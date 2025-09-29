using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_CourseScheduleRepository : IBaseRepository<ACAD_CourseSchedule>
    {
        Task<IEnumerable<ACAD_CourseSchedule>> GetSchedulesByCourseIdAsync(Guid courseId);
        Task<IEnumerable<ACAD_CourseSchedule>> GetSchedulesByDayOfWeekAsync(string dayOfWeek);
        Task<IEnumerable<ACAD_CourseSchedule>> GetSchedulesByTimeSlotIdAsync(Guid timeSlotId);
        Task<bool> IsTimeSlotAvailableAsync(Guid courseId, Guid timeSlotId, string dayOfWeek);
        Task<ACAD_CourseSchedule?> GetDetailByIdAsync(Guid id);
        Task<IEnumerable<ACAD_CourseSchedule>> GetAllWithNavigationPropertiesAsync();
    }
}
