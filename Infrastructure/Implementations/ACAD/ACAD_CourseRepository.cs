using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_CourseRepository : BaseRepository<ACAD_Course>, IACAD_CourseRepository
    {
        public ACAD_CourseRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ACAD_Course>> SearchAsync(string keyword)
        {
            return await _context.ACAD_Courses
                .Where(c => c.CourseName.Contains(keyword) || c.CourseCode.Contains(keyword))
                .ToListAsync();
        }

        public async Task<IEnumerable<ACAD_Course>> FilterAsync(Guid? levelId, Guid? formatId, Guid? teacherId)
        {
            var query = _context.ACAD_Courses.AsQueryable();

            if (levelId.HasValue)
                query = query.Where(c => c.CourseLevelID == levelId);

            if (formatId.HasValue)
                query = query.Where(c => c.CourseFormatID == formatId);

            if (teacherId.HasValue)
            {
                query = query.Where(c =>
                    _context.ACAD_CourseTeacherAssignments
                        .Any(a => a.CourseID == c.Id && a.TeacherID == teacherId));
            }

            return await query.ToListAsync();
        }

        public async Task<ACAD_Course?> GetDetailAsync(Guid courseId)
        {
            return await _context.ACAD_Courses
                .Include(c => c.Category)
                .Include(c => c.ACAD_Syllabi)
                .ThenInclude(s => s.ACAD_SyllabusItems)
                .FirstOrDefaultAsync(c => c.Id == courseId);
        }
    }
}
