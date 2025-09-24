using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_SyllabusRepository : BaseRepository<ACAD_Syllabus>, IACAD_SyllabusRepository
    {
        public ACAD_SyllabusRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<ACAD_Syllabus>> GetByCourseIdAsync(Guid courseId)
        {
            return await _context.ACAD_Syllabi
                .Where(s => s.CourseID == courseId)
                .Include(s => s.ACAD_SyllabusItems)
                .ToListAsync();
        }
    }
}


