using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_Attendance.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations.ACAD
{
    public class AttendanceService : IACAD_AttendanceService
    {
        private readonly IACAD_AttendanceRepository _attendanceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AttendanceService(
            IACAD_AttendanceRepository attendanceRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _attendanceRepository = attendanceRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AttendanceResponse> MarkAttendanceAsync(Guid meetingId, Guid studentId, Guid statusId, Guid teacherId, string? notes = null)
        {
            var existing = await _attendanceRepository.GetByMeetingAndStudentAsync(meetingId, studentId);

            if (existing == null)
            {
                var attendance = new ACAD_Attendance
                {
                    Id = Guid.NewGuid(),
                    MeetingID = meetingId,
                    StudentID = studentId,
                    AttendanceStatusID = statusId,
                    Notes = notes,
                    CheckedBy = teacherId,
                    CreatedAt = DateTime.UtcNow
                };
                _attendanceRepository.Add(attendance);
                existing = attendance;
            }
            else
            {
                existing.AttendanceStatusID = statusId;
                existing.Notes = notes;
                existing.UpdatedAt = DateTime.UtcNow;
                existing.UpdatedBy = teacherId;
                existing.CheckedBy = teacherId;
                _attendanceRepository.Update(existing);
            }

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AttendanceResponse>(existing);
        }

        public async Task<IEnumerable<AttendanceResponse>> GetAttendanceByMeetingAsync(Guid meetingId)
        {
            var list = await _attendanceRepository.GetByMeetingAsync(meetingId);
            return _mapper.Map<IEnumerable<AttendanceResponse>>(list);
        }

        public async Task<IEnumerable<AttendanceResponse>> GetAttendanceByStudentAsync(Guid studentId)
        {
            var list = await _attendanceRepository.GetByStudentAsync(studentId);
            return _mapper.Map<IEnumerable<AttendanceResponse>>(list);
        }
    }
}
