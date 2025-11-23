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
		private readonly IFAC_RoomRepository _roomRepository;

		public FAC_RoomService(IFAC_RoomRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
			: base(repository, unitOfWork, mapper)
		{
			_roomRepository = repository;
		}

		public async Task<IReadOnlyList<RoomResponse>> GetByTypeAsync(Guid roomTypeId)
		{
			var items = await _roomRepository.GetAllAsync();
			var filtered = items
				.Where(r => r.RoomTypeId == roomTypeId)
				.ToList();
			return _mapper.Map<IReadOnlyList<RoomResponse>>(filtered);
		}
	}
}



