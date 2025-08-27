using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_AssignmentRepository : BaseRepository<ACAD_Assignment>, IACAD_AssignmentRepository
    {
        public ACAD_AssignmentRepository(AppDbContext context) : base(context)
        {
        }
    }
}


