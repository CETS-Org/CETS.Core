using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.COM;
using Infrastructure.Implementations.Repositories;

namespace Infrastructure.Implementations.Repositories.COM
{
    public class COM_ConversationRepository : BaseRepository<COM_Conversation>, ICOM_ConversationRepository
    {
        public COM_ConversationRepository(AppDbContext context) : base(context)
        {
        }
    }
}


