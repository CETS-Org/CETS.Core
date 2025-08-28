using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.IDN;

namespace Infrastructure.Repositories.IDN
{
    public class IDN_AccountRoleRepository : BaseRepository<IDN_AccountRole>, IIDN_AccountRoleRepository
    {
        public IDN_AccountRoleRepository(AppDbContext context) : base(context)
        {
        }
    }
}


