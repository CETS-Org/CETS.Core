using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_AcademicRequestHistoryRepository : BaseRepository<ACAD_AcademicRequestHistory>, IACAD_AcademicRequestHistoryRepository
    {
        public ACAD_AcademicRequestHistoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}


