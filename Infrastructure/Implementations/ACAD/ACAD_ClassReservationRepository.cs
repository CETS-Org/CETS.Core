using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_ClassReservationRepository : BaseRepository<ACAD_ClassReservation>, IACAD_ClassReservationRepository
    {
        public ACAD_ClassReservationRepository(AppDbContext context) : base(context)
        {
        }
    }
}


