using Domain.Entities;
using DTOs.ACAD.ACAD_ClassMeetings.Responses;
using DTOs.ACAD.ACAD_SyllabusItem.Responses;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_ClassMeetingRepository : IBaseRepository<ACAD_ClassMeeting>
    {
        Task<ACAD_ClassMeeting?> GetClassMeetingTodayByClassId(Guid classId);
        Task<IEnumerable<StudentWeeklyScheduleResponse>> WeeklyScheduleGetByStudentAsync(Guid studentId, CancellationToken ct);
        Task<IEnumerable<TeacherWeeklyScheduleResponse>> WeeklyScheduleGetByTeacherAsync(Guid teacherId, CancellationToken ct);
        Task<IEnumerable<ACAD_ClassMeeting>> GetAllClassMeetingByClassId(Guid classId);
        Task<ACAD_SyllabusItem> GetCoveredTopicByClassMeetingId(Guid classMeetingId);
        Task<ACAD_ClassMeeting?> GetMeetingDetailAsync(Guid roomId, DateOnly date, Guid slotId);

        Task<IReadOnlyList<ACAD_ClassMeeting>> GetMeetingsForScheduleOverlapAsync(
           DateOnly startDate,
           DateOnly endDate,
           IEnumerable<Guid> slotIds);

        Task<IReadOnlyList<ACAD_ClassMeeting>> GetMeetingsForTeacherOverlapAsync(
            DateOnly startDate,
            DateOnly endDate,
            IEnumerable<Guid> slotIds,
            IEnumerable<Guid> teacherAssignmentIds);
    }
}


