using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Entities;
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
        private readonly IMapper _mapper;
        public ACAD_ClassMeetingsService(IACAD_ClassMeetingRepository classMeetingRepository, IMapper mapper)
        {
            _classMeetingRepository = classMeetingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClassMeetingResponse>> GetAllClassMeetingByClassId(Guid classId)
        {
            var result = await _classMeetingRepository.GetAllClassMeetingByClassId(classId);
            return _mapper.Map<IEnumerable<ClassMeetingResponse>>(result);
        }

        public async Task<ACAD_ClassMeeting?> GetClassMeetingTodayByClassId(Guid classId)
        {
            return await _classMeetingRepository.GetClassMeetingTodayByClassId(classId);
        }

        public async Task<IEnumerable<StudentWeeklyScheduleResponse>> WeeklyScheduleGetByStudentAsync(Guid studentId, CancellationToken ct)
        {
            return await _classMeetingRepository.WeeklyScheduleGetByStudentAsync(studentId, ct);
        }
    }
}
