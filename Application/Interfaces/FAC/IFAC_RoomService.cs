using Domain.Entities;
using DTOs.FAC.FAC_Room.Requests;
using DTOs.FAC.FAC_Room.Responses;

namespace Application.Interfaces.FAC
{
	public interface IFAC_RoomService : IBaseService<FAC_Room, RoomResponse, UpdateRoomRequest, CreateRoomRequest>
	{
		Task<IReadOnlyList<RoomResponse>> GetByTypeAsync(Guid roomTypeId);
	}
}



