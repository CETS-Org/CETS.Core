using Domain.Entities;

namespace Domain.Interfaces.FAC
{
    public interface IFAC_RoomRepository : IBaseRepository<FAC_Room>
    {
        Task<IReadOnlyList<FAC_Room>> GetByStatusAsync(Guid statusId);
        Task<List<CORE_LookUp>> GetRoomTypesAsync();
        Task<List<CORE_LookUp>> GetRoomStatusesAsync();
        Task<List<ACAD_ClassMeeting>> GetMeetingsWithNavigationAsync(DateOnly start, DateOnly end);

        Task<IReadOnlyList<FAC_Room>> GetActiveRoomsAsync();
        Task<IReadOnlyList<FAC_Room>> GetAvailableRoomsByIdsAsync(IEnumerable<Guid> roomIds);

       

    }

}



