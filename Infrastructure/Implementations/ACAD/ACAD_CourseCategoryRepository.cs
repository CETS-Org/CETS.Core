using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_CourseCategoryRepository : BaseRepository<ACAD_CourseCategory>, IACAD_CourseCategoryRepository
    {
        public ACAD_CourseCategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}


