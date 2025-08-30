using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_CourseCategoryRepository : BaseRepository<ACAD_CourseCategory>, IACAD_CourseCategoryRepository
    {
            public ACAD_CourseCategoryRepository(AppDbContext context) : base(context)
            {
            }

            public async Task<ACAD_CourseCategory?> GetByCodeAsync(string code)
            {
                return await _context.ACAD_CourseCategories
                    .FirstOrDefaultAsync(c => c.Code == code);
            }

    }
}


