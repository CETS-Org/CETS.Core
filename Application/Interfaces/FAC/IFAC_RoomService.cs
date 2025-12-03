using Domain.Entities;
using DTOs.FAC.FAC_Room.Requests;
using DTOs.FAC.FAC_Room.Responses;

namespace Application.Interfaces.FAC
{
	public interface IFAC_RoomService : IBaseService<FAC_Room, RoomResponse, UpdateRoomRequest, CreateRoomRequest>
	{
		Task<IReadOnlyList<RoomResponse>> GetByTypeAsync(Guid roomTypeId);

        Task<RoomResponse> UpdateRoomStatusAsync(Guid id, Guid statusId);
        Task<RoomResponse> PatchAsync(Guid id, UpdateRoomRequest request);

        Task<SlotAvailabilityDto> CheckSlotAvailabilityAsync(Guid roomId, DateTime date, int slotNumber);

        Task<IEnumerable<RoomResponse>> GetAvailableRoomsForSlotAsync(DateTime date, Guid slotId);

        Task<IEnumerable<RoomWeeklyScheduleDto>> GetWeeklyScheduleAsync(DateTime weekStart, DateTime weekEnd);
        Task<RoomStatisticsResponse> GetStatisticsAsync();

        Task<List<RoomTypeResponse>> GetRoomTypesAsync();
        Task<List<RoomStatusResponse>> GetRoomStatusesAsync();
        Task<RoomSlotInfoResponse> GetSlotInfoAsync(Guid roomId, DateOnly date, int slotNumber);
        Task<Guid> BookSlotAsync(BookRoomSlotRequest request);
        Task CancelSlotBookingAsync(Guid meetingId);

        Task<IReadOnlyList<RoomOptionDto>> GetAvailableRoomsAsync(GetAvailableRoomsRequest request);



    }
}



