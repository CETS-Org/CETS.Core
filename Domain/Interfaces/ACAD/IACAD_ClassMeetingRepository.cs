using Domain.Entities;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_ClassMeetingRepository : IBaseRepository<ACAD_ClassMeeting>
    {
        Task<ACAD_ClassMeeting?> GetClassMeetingTodayByClassId(Guid classId);
    }
}


