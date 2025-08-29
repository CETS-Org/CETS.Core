using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_SubmissionRepository : BaseRepository<ACAD_Submission>, IACAD_SubmissionRepository
    {
        public ACAD_SubmissionRepository(AppDbContext context) : base(context)
        {
        }
    }
}


