using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.FAC;

namespace Infrastructure.Repositories.FAC
{
    public class FAC_RoomRepository : BaseRepository<FAC_Room>, IFAC_RoomRepository
    {
        public FAC_RoomRepository(AppDbContext context) : base(context)
        {
        }
    }
}


