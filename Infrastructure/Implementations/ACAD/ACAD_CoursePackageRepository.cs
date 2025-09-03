using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_CoursePackageRepository : BaseRepository<ACAD_CoursePackage>, IACAD_CoursePackageRepository
    {
        public ACAD_CoursePackageRepository(AppDbContext context) : base(context)
        { }
            public async Task<IEnumerable<ACAD_CoursePackage>> GetActivePackagesAsync()
        {
            return await _context.ACAD_CoursePackages
                .Where(p => p.IsActive)
                .Include(p => p.ACAD_CoursePackageItems)
                    .ThenInclude(i => i.Course)
                .ToListAsync();
        }

        public async Task<ACAD_CoursePackage?> GetDetailAsync(Guid packageId)
        {
            return await _context.ACAD_CoursePackages
                .Include(p => p.ACAD_CoursePackageItems)
                    .ThenInclude(i => i.Course)
                .FirstOrDefaultAsync(p => p.Id == packageId && p.IsActive);
        }
    }
    
}


