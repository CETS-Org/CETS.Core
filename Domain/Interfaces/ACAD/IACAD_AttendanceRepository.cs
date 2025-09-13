using Domain.Entities;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_AttendanceRepository : IBaseRepository<ACAD_Attendance>
    {
        Task<IEnumerable<ACAD_Attendance>> GetByMeetingAsync(Guid meetingId);
        Task<IEnumerable<ACAD_Attendance>> GetByStudentAsync(Guid studentId);
        Task<ACAD_Attendance?> GetByMeetingAndStudentAsync(Guid meetingId, Guid studentId);
        Task<int> CountTotalMeetingsByCourseAsync(Guid courseId);
        Task<List<ACAD_Attendance>> GetByStudentAndCourseAsync(Guid studentId, Guid courseId);
    }
}


