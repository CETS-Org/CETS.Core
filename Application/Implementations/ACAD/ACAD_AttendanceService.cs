using Application.Interfaces.ACAD;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_Attendance.Requests;
using DTOs.ACAD.ACAD_Attendance.Responses;
using Microsoft.EntityFrameworkCore;
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
        private readonly IACAD_EnrollmentRepository _enrollmentRepository;
        private readonly IACAD_ClassMeetingRepository _classMeetingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AttendanceService(
            IACAD_AttendanceRepository attendanceRepository,
            IACAD_EnrollmentRepository enrollmentRepository,
            IACAD_ClassMeetingRepository classMeetingRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _attendanceRepository = attendanceRepository;
            _enrollmentRepository = enrollmentRepository;
            _classMeetingRepository = classMeetingRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AttendanceResponse> MarkAttendanceAsync(Guid meetingId, Guid studentId, Guid statusId, Guid teacherId, string? notes = null)
        {
            // Kiểm tra thời hạn điểm danh
            var meeting = await _classMeetingRepository.GetByIdAsync(meetingId);
            if (meeting == null)
            {
                throw new Exception($"Class meeting with ID {meetingId} not found");
            }

            var meetingDate = meeting.Date.ToDateTime(TimeOnly.MinValue).Date;
            var currentDate = DateTime.Now.Date;

            if (meetingDate != currentDate)
            {
                throw new Exception("Attendance can only be taken on the exact class date.");
            }

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
                    CreatedAt = DateTime.Now
                };
                _attendanceRepository.Add(attendance);
                existing = attendance;
            }
            else
            {
                existing.AttendanceStatusID = statusId;
                existing.Notes = notes;
                existing.UpdatedAt = DateTime.Now;
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
        public async Task<StudentAttendanceSummaryResponse?> GetStudentAttendanceSummaryAsync(Guid studentId, Guid courseId)
        {
            // 🔹 Lấy danh sách attendance của student trong đúng course
            var attendances = await _attendanceRepository.GetByStudentAndCourseAsync(studentId, courseId);

            // 🔹 Lấy danh sách buổi học (meetings) thuộc course này để tính tổng buổi
            var totalSessions = await _attendanceRepository.CountTotalMeetingsByCourseAsync(courseId);

            if (totalSessions == 0)
                return null;

            var present = attendances.Count(a => a.AttendanceStatus.Code == "Present");
            var absent = attendances.Count(a => a.AttendanceStatus.Code == "Absent");
            var attendanceRate = Math.Round((double)present / totalSessions * 100, 1);
            var absentRate = Math.Round((double)absent / totalSessions * 100, 1);

            var firstAttendance = attendances.FirstOrDefault();

            // 🔹 Build response
            return new StudentAttendanceSummaryResponse
            {
                StudentId = studentId,
                CourseId = courseId,
                CourseName = firstAttendance?.Meeting?.TeacherAssignment?.Course?.CourseName ?? string.Empty,
                ClassName = firstAttendance?.Meeting?.Class?.ClassName,
                TeacherName = firstAttendance?.Meeting?.TeacherAssignment?.Teacher?.Account?.FullName,
                TotalSessions = totalSessions,
                Attended = present,
                Absent = absent,
                AttendanceRate = attendanceRate,
                IsWarning = false,
                WarningMessage = null,

                // 🔹 Danh sách chi tiết các buổi học (sessions)
                SessionRecords = attendances
                    .Where(a => a.Meeting?.TeacherAssignment?.CourseID == courseId) // ✅ lọc đúng course
                    .Select(a =>
                    {
                        var startStr = a.Meeting.Slot?.Name?.Trim();
                        string? endStr = null;

                        if (TimeSpan.TryParse(startStr, out var start))
                        {
                            endStr = (start + TimeSpan.FromMinutes(90)).ToString("hh\\:mm");
                            startStr = start.ToString("hh\\:mm");
                        }

                        return new AttendanceDetailResponse
                        {
                            MeetingId = a.MeetingID,
                            MeetingDate = a.Meeting?.Date.ToDateTime(TimeOnly.MinValue) ?? DateTime.MinValue,
                            Status = a.AttendanceStatus?.Code ?? "N/A",
                            Notes = a.Notes,
                            TopicTitle = a.Meeting?.CoveredTopic?.TopicTitle ?? string.Empty,
                            RoomCode = a.Meeting?.Room?.RoomCode,
                            StartTime = startStr,
                            EndTime = endStr,
                            CheckedBy = a.CheckedByNavigation?.Account?.FullName
                        };
                    })
                    .OrderBy(r => r.MeetingDate)
                    .ToList()
            };
        }


        public async Task<List<StudentAttendanceSummaryResponse>> GetStudentAttendanceReportAsync(Guid studentId)
        {
            var enrollments = await _enrollmentRepository.GetByStudentAsync(studentId);
            var totalClasses = enrollments.Select(e => e.ClassID).Distinct().Count();

            var result = new List<StudentAttendanceSummaryResponse>();

            foreach (var e in enrollments)
            {
                var totalSessions = await _attendanceRepository.CountTotalMeetingsByClassAsync(e.ClassID);
                var attendances = await _attendanceRepository.GetByStudentAndClassAsync(studentId, e.ClassID);

                var present = attendances.Count(a => a.AttendanceStatus.Code == "Present");
                var absent = attendances.Count(a => a.AttendanceStatus.Code == "Absent");
                var rate = totalSessions == 0 ? 0 : Math.Round((double)present / totalSessions * 100, 1);

                var sessionRecords = attendances.Select(a =>
                {

                    var startStr = a.Meeting.Slot?.Name?.Trim();
                    string? endStr = null;

                    if (TimeSpan.TryParse(startStr, out var start))
                    {
                        endStr = (start + TimeSpan.FromMinutes(90)).ToString(@"hh\:mm");
                        startStr = start.ToString(@"hh\:mm");
                    }

                    return new AttendanceDetailResponse
                    {
                        MeetingId = a.MeetingID,
                        MeetingDate = a.Meeting?.Date.ToDateTime(TimeOnly.MinValue) ?? DateTime.MinValue,
                        Status = a.AttendanceStatus?.Code ?? "N/A",
                        Notes = a.Notes,
                        TopicTitle = a.Meeting?.CoveredTopic?.TopicTitle ?? string.Empty,
                        RoomCode = a.Meeting?.Room?.RoomCode,
                        StartTime = startStr,
                        EndTime = endStr,
                        CheckedBy = a.CheckedByNavigation?.Account?.FullName
                    };
                })
                .OrderBy(r => r.MeetingDate)
                .ToList();

                result.Add(new StudentAttendanceSummaryResponse
                {
                    StudentId = studentId,
                    CourseId = e.CourseID,
                    CourseName = e.Course?.CourseName ?? "(No name)",
                    ClassName = e.Class?.ClassName,
                    TeacherName = attendances.FirstOrDefault()?.Meeting?.TeacherAssignment?.Teacher?.Account?.FullName,
                    TotalClasses = totalClasses,
                    TotalSessions = totalSessions,
                    Attended = present,
                    Absent = absent,
                    AttendanceRate = rate,
                    SessionRecords = sessionRecords,
                    IsWarning = totalSessions > 0 && (double)absent / totalSessions * 100 > 30,
                    WarningMessage = totalSessions > 0 && (double)absent / totalSessions * 100 > 30
                        ? $"You have been absent {absent}/{totalSessions} sessions ({(double)absent / totalSessions * 100:F1}%). Maximum allowed is 30%."
                        : null
                });
            }

            return result;
        }



        public async Task<IEnumerable<StudentAttendanceListResponse>> GetStudentsByClassForAttendanceAsync(Guid classId, Guid? classMeetingId = null)
        {
            return await _attendanceRepository.GetStudentsByClassForAttendanceAsync(classId, classMeetingId);
        }

        public async Task<BulkAttendanceResponse> BulkMarkAttendanceAsync(BulkAttendanceRequest request)
        {
            return await _attendanceRepository.BulkMarkAttendanceAsync(request);
        }

    }

}

