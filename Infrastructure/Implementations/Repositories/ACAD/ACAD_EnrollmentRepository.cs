using Domain.Constants;
using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Domain.Interfaces.CORE;
using Infrastructure.Implementations.Repositories;
using Infrastructure.Implementations.Repositories.CORE;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_EnrollmentRepository : BaseRepository<ACAD_Enrollment>, IACAD_EnrollmentRepository
    {
        private readonly ICORE_LookUpRepository _lookUpRepository;
        public ACAD_EnrollmentRepository(AppDbContext context, ICORE_LookUpRepository lookUpRepository) : base(context)
        {
            _lookUpRepository = lookUpRepository;
        }
        public async Task<IEnumerable<ACAD_Enrollment>> GetAllEnrollment()
        {
            return await _context.ACAD_Enrollments
                .Include(e => e.Class)
                .Include(e => e.Course)
                    .ThenInclude(c => c.ACAD_CourseTeacherAssignments)
                        .ThenInclude(cta => cta.Teacher)
                        .ThenInclude(t => t.Account)
                .Include(e => e.EnrollmentStatus)
                .ToListAsync();
        }
        public async Task<IEnumerable<ACAD_Enrollment>> GetByStudentAsync(Guid studentId)
        {
            return await _context.ACAD_Enrollments
                .Include(e => e.Class)
                .Include(e => e.Course)
                    .ThenInclude(c => c.ACAD_CourseTeacherAssignments)
                        .ThenInclude(cta => cta.Teacher)
                        .ThenInclude(t => t.Account)
                .Include(e => e.Course)
                    .ThenInclude(c => c.ACAD_CourseSchedules)
                .Include(e => e.EnrollmentStatus)
                .Where(e => e.StudentID == studentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ACAD_Enrollment>> GetByClassAsync(Guid classId)
        {
            return await _context.ACAD_Enrollments
                .Include(e => e.Student)
                    .ThenInclude(s => s.Account)
                .Where(e => e.ClassID == classId)
                .ToListAsync();
        }

        public async Task<ACAD_Enrollment?> GetDetailAsync(Guid enrollmentId)
        {
            return await _context.ACAD_Enrollments
                .Include(e => e.Student)
                .Include(e => e.Class)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == enrollmentId);
        }

        public async Task<ACAD_Enrollment?> GetEnrollmentForRefundAsync(Guid enrollmentId)
        {
            return await _context.ACAD_Enrollments
                .Include(e => e.Student)
                    .ThenInclude(s => s.Account)
                .Include(e => e.Course)
                .Include(e => e.Invoice)
                    .ThenInclude(i => i.FIN_Payments)
                .FirstOrDefaultAsync(e => e.Id == enrollmentId);
        }

        public async Task<IEnumerable<ACAD_Enrollment>> GetStudentAcademicResultsAsync(Guid studentId)
        {
            return await _context.ACAD_Enrollments
                .AsNoTracking()
                .Include(e => e.Course)
                    .ThenInclude(c => c.ACAD_CourseTeacherAssignments)
                        .ThenInclude(cta => cta.Teacher)
                            .ThenInclude(t => t.Account)
                .Include(e => e.EnrollmentStatus)
                .Where(e => e.StudentID == studentId && !e.IsDeleted)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }


        //View Course Detail in Academic Results
        public async Task<ACAD_Enrollment?> GetEnrollmentDetailByStudentAndCourseAsync(Guid studentId, Guid courseId)
        {
            return await _context.ACAD_Enrollments
                .AsNoTracking()
                .Include(e => e.Course)
                    .ThenInclude(c => c.ACAD_CourseTeacherAssignments)
                        .ThenInclude(cta => cta.Teacher)
                            .ThenInclude(t => t.Account)
                .Include(e => e.EnrollmentStatus)
                .Include(e => e.Class)
                    .ThenInclude(c => c.ACAD_ClassMeetings)
                        .ThenInclude(m => m.ACAD_Assignments)
                            .ThenInclude(a => a.ACAD_Submissions)
                .FirstOrDefaultAsync(e =>
                    e.StudentID == studentId &&
                    e.CourseID == courseId &&
                    !e.IsDeleted);
        }

        public async Task<IEnumerable<ACAD_Enrollment>> GetStudentWaitList(Guid courseId)
        {

            // Nên đưa ra constant file
            var waitingStatusId = await _lookUpRepository.GetByCodeAsync("EnrollmentStatus", "Pending"); //Guid.Parse("2dba3beb-8336-417f-9dc3-fb853604dd2f");

            return await _context.ACAD_Enrollments
                .AsNoTracking() // Tối ưu tốc độ đọc
                .Include(e => e.Student)            // Lấy thông tin học sinh
                    .ThenInclude(s => s.Account)    // Lấy tiếp thông tin tài khoản (Tên, Email...)
                .Where(e =>
                    e.CourseID == courseId &&
                    e.ClassID == null &&            // Chưa xếp lớp
                    !e.IsDeleted &&                 // Enrollment chưa bị xóa
                    e.EnrollmentStatusID == waitingStatusId.Id &&
                    !e.Student.IsDeleted            // Quan trọng: Học sinh cũng phải chưa bị xóa
                )
                .OrderBy(e => e.CreatedAt)          // (Tùy chọn) Sắp xếp theo ai đăng ký trước xếp trước
                .ToListAsync();
        }

        public async Task UpdateDecisionStatusAsync(Guid enrollmentId, EmailDecisionStatus status)
        {
            var entity = await _context.ACAD_Enrollments.FirstOrDefaultAsync(e => e.Id == enrollmentId);

            if (entity != null)
            {
                entity.EmailDecisionStatus = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ACAD_Enrollment?> GetEnrollmentWithCourseAsync(Guid enrollmentId)
        {
            return await _context.ACAD_Enrollments
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == enrollmentId);
        }

    }
}


