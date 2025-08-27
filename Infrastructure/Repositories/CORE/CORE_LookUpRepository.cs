using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.CORE;

namespace Infrastructure.Repositories.CORE
{
    public class CORE_LookUpRepository : BaseRepository<CORE_LookUp>, ICORE_LookUpRepository
    {
        public CORE_LookUpRepository(AppDbContext context) : base(context)
        {
        }
    }
}


