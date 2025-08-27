using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.CORE;

namespace Infrastructure.Repositories.CORE
{
    public class CORE_LookUpTypeRepository : BaseRepository<CORE_LookUpType>, ICORE_LookUpTypeRepository
    {
        public CORE_LookUpTypeRepository(AppDbContext context) : base(context)
        {
        }
    }
}


