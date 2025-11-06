using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_Assignment.Responses;
using DTOs.ACAD.ACAD_Class.Requests;
using DTOs.ACAD.ACAD_ClassMeetings.Requests;
using DTOs.ACAD.ACAD_ClassMeetings.Responses;
using DTOs.ACAD.ACAD_SyllabusItem.Responses;
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
        private readonly IUnitOfWork _uow;
        public ACAD_ClassMeetingsService(IACAD_ClassMeetingRepository classMeetingRepository, IMapper mapper, IUnitOfWork uow)
        {
            _classMeetingRepository = classMeetingRepository;
            _mapper = mapper;
            _uow = uow;
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

        public async Task<SyllabusItemResponse?> GetCoveredTopicAsync(Guid classMeetingId)
        {
            var result = await _classMeetingRepository.GetCoveredTopicByClassMeetingId(classMeetingId);
            return _mapper.Map<SyllabusItemResponse>(result);
        }

        public async Task<IEnumerable<StudentWeeklyScheduleResponse>> WeeklyScheduleGetByStudentAsync(Guid studentId, CancellationToken ct)
        {
            return await _classMeetingRepository.WeeklyScheduleGetByStudentAsync(studentId, ct);
        }

        public async Task<IEnumerable<TeacherWeeklyScheduleResponse>> WeeklyScheduleGetByTeacherAsync(Guid teacherId, CancellationToken ct)
        {
            return await _classMeetingRepository.WeeklyScheduleGetByTeacherAsync(teacherId, ct);
        }

        public async Task<Guid> CreateClassMeetingAsync(CreateClassMeetingRequest request)
        {
            return await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = _mapper.Map<ACAD_ClassMeeting>(request);
                entity.Id = Guid.NewGuid();
                entity.ClassID = request.ClassID;
                entity.SlotID = request.SlotID;
                entity.Date = request.Date;
                entity.RoomID = request.RoomID;
                entity.TeacherAssignmentID = request.TeacherAssignmentID;
                entity.OnlineMeetingUrl = request.OnlineMeetingUrl;
                entity.Passcode = request.Passcode;
                entity.CoveredTopicID = request.CoveredTopicID;                           

                _classMeetingRepository.Add(entity);
                await _uow.SaveChangesAsync();

                return entity.Id;
            });
        }

        public async Task UpdateClassMeetingAsync(UpdateClassMeetingRequest request)
        {
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                var entity = await _classMeetingRepository.GetByIdAsync(request.Id);
                if (entity == null) throw new Exception("ClassMeeting not found");

                _mapper.Map(request, entity);
                entity.UpdatedAt = DateTime.UtcNow;

                _classMeetingRepository.Update(entity);
                await _uow.SaveChangesAsync();
            });
        }


    }
}
