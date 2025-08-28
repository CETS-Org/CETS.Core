using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_EnrollmentRepository : BaseRepository<ACAD_Enrollment>, IACAD_EnrollmentRepository
    {
        public ACAD_EnrollmentRepository(AppDbContext context) : base(context)
        {
        }
    }
}


