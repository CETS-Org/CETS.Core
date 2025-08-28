using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.IDN;

namespace Infrastructure.Repositories.IDN
{
    public class IDN_StudentRepository : BaseRepository<IDN_Student>, IIDN_StudentRepository
    {
        public IDN_StudentRepository(AppDbContext context) : base(context)
        {
        }
    }
}


