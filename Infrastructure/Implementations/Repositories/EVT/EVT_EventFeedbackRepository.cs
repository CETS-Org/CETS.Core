using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.EVT;
using Infrastructure.Implementations.Repositories;

namespace Infrastructure.Implementations.Repositories.EVT
{
    public class EVT_EventFeedbackRepository : BaseRepository<EVT_EventFeedback>, IEVT_EventFeedbackRepository
    {
        public EVT_EventFeedbackRepository(AppDbContext context) : base(context)
        {
        }
    }
}


