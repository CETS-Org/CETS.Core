using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_CoursePackageItemRepository : BaseRepository<ACAD_CoursePackageItem>, IACAD_CoursePackageItemRepository
    {
        public ACAD_CoursePackageItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}


