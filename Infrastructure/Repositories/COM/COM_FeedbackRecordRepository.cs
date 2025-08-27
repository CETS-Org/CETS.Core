using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.COM;

namespace Infrastructure.Repositories.COM
{
    public class COM_FeedbackRecordRepository : BaseRepository<COM_FeedbackRecord>, ICOM_FeedbackRecordRepository
    {
        public COM_FeedbackRecordRepository(AppDbContext context) : base(context)
        {
        }
    }
}


