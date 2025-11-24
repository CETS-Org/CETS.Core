using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_EnrollmentRepository : BaseRepository<ACAD_Enrollment>, IACAD_EnrollmentRepository
    {
        public ACAD_EnrollmentRepository(AppDbContext context) : base(context)
        {
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

    }
}


