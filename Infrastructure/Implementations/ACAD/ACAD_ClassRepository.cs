using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_ClassRepository : BaseRepository<ACAD_Class>, IACAD_ClassRepository
    {
        public ACAD_ClassRepository(AppDbContext context) : base(context)
        {
        }
    }
}


