using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.EVT;
using Infrastructure.Implementations.Repositories;

namespace Infrastructure.Implementations.Repositories.EVT
{
    public class EVT_EventRepository : BaseRepository<EVT_Event>, IEVT_EventRepository
    {
        public EVT_EventRepository(AppDbContext context) : base(context)
        {
        }
    }
}


