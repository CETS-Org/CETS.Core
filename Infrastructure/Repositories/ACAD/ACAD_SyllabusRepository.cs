using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_SyllabusRepository : BaseRepository<ACAD_Syllabus>, IACAD_SyllabusRepository
    {
        public ACAD_SyllabusRepository(AppDbContext context) : base(context)
        {
        }
    }
}


