using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_CourseRepository : BaseRepository<ACAD_Course>, IACAD_CourseRepository
    {
        public ACAD_CourseRepository(AppDbContext context) : base(context)
        {
        }
    }
}


