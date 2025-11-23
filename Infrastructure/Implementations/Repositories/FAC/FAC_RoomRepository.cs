using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.FAC;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.FAC
{
    public class FAC_RoomRepository : BaseRepository<FAC_Room>, IFAC_RoomRepository
    {
        public FAC_RoomRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<IReadOnlyList<FAC_Room>> GetAllAsync()
        {
            return await _context.FAC_Rooms
                .Include(r => r.RoomType)
                .Include(r => r.RoomStatus)
                .ToListAsync();
        }

        public override async Task<FAC_Room?> GetByIdAsync(Guid id)
        {
            return await _context.FAC_Rooms
                .Include(r => r.RoomType)
                .Include(r => r.RoomStatus)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}


