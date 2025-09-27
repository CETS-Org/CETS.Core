using Application.Interfaces.ACAD;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_ClassMeetings.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.ACAD
{
    public class ACAD_ClassMeetingsService : IACAD_ClassMeetingsService
    {
        private readonly IACAD_ClassMeetingRepository _classMeetingRepository;
        public ACAD_ClassMeetingsService(IACAD_ClassMeetingRepository classMeetingRepository)
        {
            _classMeetingRepository = classMeetingRepository;
        }
        public async Task<IEnumerable<StudentWeeklyScheduleResponse>> WeeklyScheduleGetByStudentAsync(Guid studentId, CancellationToken ct)
        {
            return await _classMeetingRepository.WeeklyScheduleGetByStudentAsync(studentId, ct);
        }
    }
}
