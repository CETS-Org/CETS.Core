using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_EnrollmentRepository : BaseRepository<ACAD_Enrollment>, IACAD_EnrollmentRepository
    {
        public ACAD_EnrollmentRepository(AppDbContext context) : base(context)
        {
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
    }
}


