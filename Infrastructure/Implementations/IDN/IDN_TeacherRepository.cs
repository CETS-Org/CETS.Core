using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.IDN;

namespace Infrastructure.Repositories.IDN
{
    public class IDN_TeacherRepository : BaseRepository<IDN_Teacher>, IIDN_TeacherRepository
    {
        public IDN_TeacherRepository(AppDbContext context) : base(context)
        {
        }
    }
}


