using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_CourseTeacherAssignmentRepository : BaseRepository<ACAD_CourseTeacherAssignment>, IACAD_CourseTeacherAssignmentRepository
    {
        public ACAD_CourseTeacherAssignmentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ACAD_Course>> GetCoursesByTeacherIdAsync(Guid teacherId)
        {
            return await _context.ACAD_Courses
                .Where(c => c.ACAD_CourseTeacherAssignments
                    .Any(cta => cta.TeacherID == teacherId))
                .Include(c => c.Category)
                .Include(c => c.CourseLevel)
                .Include(c => c.CourseFormat)
                .Include(c => c.ACAD_Enrollments)
                .ToListAsync();
        }
    }
}


