using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.COM;

namespace Infrastructure.Repositories.COM
{
    public class COM_NotificationRepository : BaseRepository<COM_Notification>, ICOM_NotificationRepository
    {
        public COM_NotificationRepository(AppDbContext context) : base(context)
        {
        }
    }
}


