using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.CORE;
using Domain.Interfaces.FAC;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.FAC
{
    public class FAC_RoomRepository : BaseRepository<FAC_Room>, IFAC_RoomRepository
    {
        private readonly ICORE_LookUpRepository _lookUpRepository;
        public FAC_RoomRepository(AppDbContext context, ICORE_LookUpRepository lookUpRepository) : base(context)
        {
            _lookUpRepository = lookUpRepository;
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
        public async Task<IReadOnlyList<FAC_Room>> GetByStatusAsync(Guid statusId)
        {
            return await _context.FAC_Rooms
                .Where(x => x.RoomStatusId == statusId)
                .ToListAsync();
        }

        public async Task<List<CORE_LookUp>> GetRoomTypesAsync()
        {
            return await _context.CORE_LookUps
                .Where(x => x.LookUpType.Code == "RoomType" && x.IsActive)
                .ToListAsync();
        }

        public async Task<List<CORE_LookUp>> GetRoomStatusesAsync()
        {
            return await _context.CORE_LookUps
                .Where(x => x.LookUpType.Code == "RoomStatus" && x.IsActive)
                .ToListAsync();
        }
        public async Task<List<ACAD_ClassMeeting>> GetMeetingsWithNavigationAsync(DateOnly start, DateOnly end)
        {
            return await _context.ACAD_ClassMeetings
                .Include(m => m.Slot)
                .Include(m => m.Class)
                .Include(m => m.TeacherAssignment)
                    .ThenInclude(t => t.Teacher)
                        .ThenInclude(t => t.Account)
                .Include(m => m.TeacherAssignment)
                    .ThenInclude(t => t.Course)
                .Where(m => m.Date >= start && m.Date <= end && m.RoomID != null)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<FAC_Room>> GetActiveRoomsAsync()
        {
            
            return await _context.FAC_Rooms
                .Where(r => r.IsActive)
                .OrderBy(r => r.RoomCode)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<FAC_Room>> GetAvailableRoomsByIdsAsync(IEnumerable<Guid> roomIds)
        {
            var ids = roomIds.ToList();

            if (!ids.Any())
                return Array.Empty<FAC_Room>();

            return await _context.FAC_Rooms
                .Where(r =>
                    ids.Contains(r.Id) &&
                    r.IsActive 
                    
                )
                .OrderBy(r => r.RoomCode)
                .ToListAsync();
        }



    }
}


