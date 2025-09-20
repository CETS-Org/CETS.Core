using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.COM;
using Infrastructure.Implementations.Repositories;

namespace Infrastructure.Implementations.Repositories.COM
{
    public class COM_FeedbackRepository : BaseRepository<COM_Feedback>, ICOM_FeedbackRepository
    {
        public COM_FeedbackRepository(AppDbContext context) : base(context)
        {
        }
    }
}


