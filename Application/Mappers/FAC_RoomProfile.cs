using AutoMapper;
using Domain.Entities;
using DTOs.FAC.FAC_Room.Requests;
using DTOs.FAC.FAC_Room.Responses;

namespace Application.Mappers
{
	public class FAC_RoomProfile : Profile
	{
		public FAC_RoomProfile()
		{
			CreateMap<FAC_Room, RoomResponse>().ReverseMap();
			CreateMap<CreateRoomRequest, FAC_Room>();
			CreateMap<UpdateRoomRequest, FAC_Room>();
		}
	}
}



