using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.EVT;
using Infrastructure.Implementations.Repositories;

namespace Infrastructure.Implementations.Repositories.EVT
{
    public class EVT_EventRegistrationRepository : BaseRepository<EVT_EventRegistration>, IEVT_EventRegistrationRepository
    {
        public EVT_EventRegistrationRepository(AppDbContext context) : base(context)
        {
        }
    }
}


