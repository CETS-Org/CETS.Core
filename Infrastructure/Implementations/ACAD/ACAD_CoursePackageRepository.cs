using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_CoursePackageRepository : BaseRepository<ACAD_CoursePackage>, IACAD_CoursePackageRepository
    {
        public ACAD_CoursePackageRepository(AppDbContext context) : base(context)
        {
        }
    }
}


