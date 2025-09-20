using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.COM;
using Infrastructure.Implementations.Repositories;

namespace Infrastructure.Implementations.Repositories.COM
{
    public class COM_NotificationRepository : BaseRepository<COM_Notification>, ICOM_NotificationRepository
    {
        public COM_NotificationRepository(AppDbContext context) : base(context)
        {
        }
    }
}


