using Domain.Entities;
using DTOs.ACAD.ACAD_Attendance.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_AttendanceService
    {
        Task<AttendanceResponse> MarkAttendanceAsync(Guid meetingId, Guid studentId, Guid statusId, Guid teacherId, string? notes = null);
        Task<IEnumerable<AttendanceResponse>> GetAttendanceByMeetingAsync(Guid meetingId);
        Task<IEnumerable<AttendanceResponse>> GetAttendanceByStudentAsync(Guid studentId);
    }
}
