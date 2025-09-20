using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_CoursePackageItemRepository : BaseRepository<ACAD_CoursePackageItem>, IACAD_CoursePackageItemRepository
    {
        public ACAD_CoursePackageItemRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<ACAD_CoursePackageItem>> GetByPackageIdAsync(Guid packageId)
        {
            return await _context.ACAD_CoursePackageItems
                .Where(i => i.PackageID == packageId)
                .Include(i => i.Course)
                .OrderBy(i => i.Sequence)
                .ToListAsync();
        }
    }
}


