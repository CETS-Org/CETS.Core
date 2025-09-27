using Domain.Entities;
using DTOs.ACAD.ACAD_ClassMeetings.Responses;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_ClassMeetingRepository : IBaseRepository<ACAD_ClassMeeting>
    {
        Task<ACAD_ClassMeeting?> GetClassMeetingTodayByClassId(Guid classId);
        Task<IEnumerable<StudentWeeklyScheduleResponse>> WeeklyScheduleGetByStudentAsync(Guid studentId, CancellationToken ct);
    }
}


