using Application.Interfaces.FAC;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.FAC;
using DTOs.FAC.FAC_Room.Requests;
using DTOs.FAC.FAC_Room.Responses;

namespace Application.Implementations.FAC
{
	public class FAC_RoomService : BaseService<FAC_Room, RoomResponse, UpdateRoomRequest, CreateRoomRequest>, IFAC_RoomService
	{
		public FAC_RoomService(IFAC_RoomRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
			: base(repository, unitOfWork, mapper)
		{
		}

		public async Task<IReadOnlyList<RoomResponse>> GetByTypeAsync(Guid roomTypeId)
		{
			var items = await _repository.FindAsync(r => r.RoomTypeId == roomTypeId);
			return _mapper.Map<IReadOnlyList<RoomResponse>>(items);
		}
	}
}



