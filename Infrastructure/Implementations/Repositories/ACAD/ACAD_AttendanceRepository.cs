using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_Attendance.Requests;
using DTOs.ACAD.ACAD_Attendance.Responses;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_AttendanceRepository : BaseRepository<ACAD_Attendance>, IACAD_AttendanceRepository
    {
        public ACAD_AttendanceRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<ACAD_Attendance>> GetByMeetingAsync(Guid meetingId)
        {
            return await _context.ACAD_Attendances
                .Where(a => a.MeetingID == meetingId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ACAD_Attendance>> GetByStudentAsync(Guid studentId)
        {
            return await _context.ACAD_Attendances
                .Where(a => a.StudentID == studentId)
                .ToListAsync();
        }

        public async Task<ACAD_Attendance?> GetByMeetingAndStudentAsync(Guid meetingId, Guid studentId)
        {
            return await _context.ACAD_Attendances
                .FirstOrDefaultAsync(a => a.MeetingID == meetingId && a.StudentID == studentId);
        }

        public async Task<int> CountTotalMeetingsByCourseAsync(Guid courseId)
        {
            return await _context.ACAD_SyllabusItems
                .Where(i => i.Syllabus != null &&
                            !i.IsDeleted &&
                            i.Syllabus.CourseID == courseId &&
                            !i.Syllabus.IsDeleted)
                .CountAsync();
        }


        public async Task<List<ACAD_Attendance>> GetByStudentAndCourseAsync(Guid studentId, Guid courseId)
        {
            return await _context.ACAD_Attendances
                .Include(a => a.AttendanceStatus)
                .Include(a => a.CheckedByNavigation)
                    .ThenInclude(u => u.Account)
                .Include(a => a.Meeting)
                    .ThenInclude(c => c.Class)
                .Include(a => a.Meeting)
                    .ThenInclude(m => m.CoveredTopic)
                .Include(a => a.Meeting)
                    .ThenInclude(m => m.Slot)
                .Include(a => a.Meeting)
                    .ThenInclude(m => m.Room)
                .Include(a => a.Meeting)
                    .ThenInclude(m => m.TeacherAssignment)
                        .ThenInclude(t => t.Course)
                .Where(a => a.StudentID == studentId &&
                            a.Meeting.TeacherAssignment != null &&
                            a.Meeting.TeacherAssignment.CourseID == courseId)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentAttendanceListResponse>> GetStudentsByClassForAttendanceAsync(Guid classId, Guid? classMeetingId = null)
        {
            // Lấy danh sách học sinh trong lớp
            var enrollments = await _context.ACAD_Enrollments
                .Include(e => e.Student)
                    .ThenInclude(s => s.Account)
                .Include(e => e.EnrollmentStatus)
                .Where(e => e.ClassID == classId && !e.IsDeleted)
                .ToListAsync();

            // Lấy status "Absent" từ lookup để làm default
            var absentStatus = await _context.CORE_LookUps
                .FirstOrDefaultAsync(l => l.Code == "Absent" && l.LookUpType.Code == "AttendanceStatus");

            var absentStatusName = absentStatus?.Name ?? "Absent";

            // Nếu có classMeetingId, lấy thông tin điểm danh của buổi học đó
            List<ACAD_Attendance>? attendances = null;
            if (classMeetingId.HasValue)
            {
                attendances = await _context.ACAD_Attendances
                    .Include(a => a.AttendanceStatus)
                    .Where(a => a.MeetingID == classMeetingId.Value)
                    .ToListAsync();
            }

            // Map sang response
            var students = enrollments.Select(e =>
            {
                // Tìm attendance record nếu có
                var attendance = attendances?.FirstOrDefault(a => a.StudentID == e.StudentID);

                return new StudentAttendanceListResponse
                {
                    StudentId = e.StudentID,
                    StudentCode = e.Student.StudentCode,
                    StudentName = e.Student.Account.FullName,
                    Email = e.Student.Account.Email,
                    PhoneNumber = e.Student.Account.PhoneNumber,
                    AvatarUrl = e.Student.Account.AvatarUrl,
                    EnrollmentId = e.Id,
                    EnrollmentStatus = e.EnrollmentStatus.Name,
                    
                    // Thông tin điểm danh
                    AttendanceId = attendance?.Id,
                    AttendanceStatus = attendance?.AttendanceStatus?.Name ?? absentStatusName,
                    AttendanceNotes = attendance?.Notes,
                    HasAttended = attendance != null
                };
            })
            .OrderBy(s => s.StudentCode)
            .ToList();

            return students;
        }

        public async Task<BulkAttendanceResponse> BulkMarkAttendanceAsync(BulkAttendanceRequest request)
        {
            // 1. Lấy thông tin class meeting để tìm classId
            var classMeeting = await _context.ACAD_ClassMeetings
                .Include(cm => cm.Class)
                .FirstOrDefaultAsync(cm => cm.Id == request.ClassMeetingId);

            if (classMeeting == null)
                throw new Exception($"Class meeting with ID {request.ClassMeetingId} not found");

            // 2. Lấy danh sách tất cả học sinh trong lớp từ enrollment
            var allStudents = await _context.ACAD_Enrollments
                .Include(e => e.Student)
                    .ThenInclude(s => s.Account)
                .Where(e => e.ClassID == classMeeting.ClassID && !e.IsDeleted)
                .Select(e => new 
                { 
                    e.StudentID,
                    e.Student.StudentCode,
                    e.Student.Account.FullName
                })
                .ToListAsync();

            // 3. Lấy Present và Absent status từ lookup
            var presentStatus = await _context.CORE_LookUps
                .FirstOrDefaultAsync(l => l.Code == "Present" && l.LookUpType.Code == "AttendanceStatus");
            var absentStatus = await _context.CORE_LookUps
                .FirstOrDefaultAsync(l => l.Code == "Absent" && l.LookUpType.Code == "AttendanceStatus");

            if (presentStatus == null || absentStatus == null)
                throw new Exception("Attendance status (Present/Absent) not found in lookup table");

            // 4. Lấy thông tin teacher để response
            var teacher = await _context.IDN_Teachers
                .Include(t => t.Account)
                .FirstOrDefaultAsync(t => t.Id == request.TeacherId);

            var teacherName = teacher?.Account?.FullName ?? "Unknown";

            // 5. Tạo hoặc update attendance records
            var records = new List<AttendanceRecordResponse>();
            var now = DateTime.UtcNow;

            foreach (var student in allStudents)
            {
                var isAbsent = request.AbsentStudentIds.Contains(student.StudentID);
                var statusId = isAbsent ? absentStatus.Id : presentStatus.Id;
                var statusCode = isAbsent ? "Absent" : "Present";

                // Kiểm tra xem đã có attendance record chưa
                var existing = await _context.ACAD_Attendances
                    .FirstOrDefaultAsync(a => a.MeetingID == request.ClassMeetingId 
                                           && a.StudentID == student.StudentID);

                if (existing == null)
                {
                    // Tạo mới
                    var attendance = new ACAD_Attendance
                    {
                        Id = Guid.NewGuid(),
                        MeetingID = request.ClassMeetingId,
                        StudentID = student.StudentID,
                        AttendanceStatusID = statusId,
                        Notes = request.Notes,
                        CheckedBy = request.TeacherId,
                        CreatedAt = now
                    };
                    _context.ACAD_Attendances.Add(attendance);

                    records.Add(new AttendanceRecordResponse
                    {
                        AttendanceId = attendance.Id,
                        StudentId = student.StudentID,
                        StudentCode = student.StudentCode,
                        StudentName = student.FullName,
                        Status = statusCode
                    });
                }
                else
                {
                    // Update existing
                    existing.AttendanceStatusID = statusId;
                    existing.Notes = request.Notes;
                    existing.CheckedBy = request.TeacherId;
                    existing.UpdatedAt = now;
                    existing.UpdatedBy = request.TeacherId;

                    records.Add(new AttendanceRecordResponse
                    {
                        AttendanceId = existing.Id,
                        StudentId = student.StudentID,
                        StudentCode = student.StudentCode,
                        StudentName = student.FullName,
                        Status = statusCode
                    });
                }
            }

            await _context.SaveChangesAsync();

            // 6. Tạo response
            var response = new BulkAttendanceResponse
            {
                ClassMeetingId = request.ClassMeetingId,
                TotalStudents = allStudents.Count,
                PresentCount = allStudents.Count - request.AbsentStudentIds.Count,
                AbsentCount = request.AbsentStudentIds.Count,
                MarkedAt = now,
                MarkedByTeacher = teacherName,
                Records = records
            };

            return response;
        }
    }
}


