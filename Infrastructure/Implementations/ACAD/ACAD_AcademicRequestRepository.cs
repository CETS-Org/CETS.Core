using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_AcademicRequestRepository : BaseRepository<ACAD_AcademicRequest>, IACAD_AcademicRequestRepository
    {
        public ACAD_AcademicRequestRepository(AppDbContext context) : base(context)
        {
        }
    }
}


