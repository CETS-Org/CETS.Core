using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.IDN;

namespace Infrastructure.Repositories.IDN
{
    public class IDN_RoleRepository : BaseRepository<IDN_Role>, IIDN_RoleRepository
    {
        public IDN_RoleRepository(AppDbContext context) : base(context)
        {
        }
    }
}


