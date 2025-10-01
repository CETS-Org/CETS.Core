using Domain.Entities;
using DTOs.ACAD.ACAD_ClassMeetings.Responses;
using DTOs.ACAD.ACAD_SyllabusItem.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_ClassMeetingsService
    {
        Task<ACAD_ClassMeeting?> GetClassMeetingTodayByClassId(Guid classId);
        Task<IEnumerable<StudentWeeklyScheduleResponse>> WeeklyScheduleGetByStudentAsync(Guid studentId, CancellationToken ct);
        Task<IEnumerable<ClassMeetingResponse>> GetAllClassMeetingByClassId(Guid classId);
        Task<SyllabusItemResponse?> GetCoveredTopicAsync(Guid classMeetingId);

    }
}
