using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.ACAD
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
        public async Task<IEnumerable<ACAD_CourseTeacherAssignment>> GetCourseTeacherAssignmentsByTeacherIdAsync(Guid teacherId)
        {
            return await _context.ACAD_CourseTeacherAssignments
                .Where(cta => cta.TeacherID == teacherId)
                .Include(cta => cta.Course)
                    .ThenInclude(c => c.Category)
                .Include(cta => cta.Course)
                    .ThenInclude(c => c.CourseLevel)
                .Include(cta => cta.Course)
                    .ThenInclude(c => c.CourseFormat)
                .Include(cta => cta.ACAD_ClassMeetings)
                    .ThenInclude(cm => cm.Room)
                .Include(cta => cta.ACAD_ClassMeetings)
                    .ThenInclude(cm => cm.Class)
                .ToListAsync();
        }
    }
}


