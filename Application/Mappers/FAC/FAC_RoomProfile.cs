using AutoMapper;
using Domain.Entities;
using DTOs.FAC.FAC_Room.Requests;
using DTOs.FAC.FAC_Room.Responses;

namespace Application.Mappers.FAC
{
	public class FAC_RoomProfile : Profile
	{
		public FAC_RoomProfile()
		{
			CreateMap<FAC_Room, RoomResponse>()
				.ForMember(dest => dest.RoomTypeName, opt => opt.MapFrom(src => src.RoomType.Name))
				.ForMember(dest => dest.RoomStatusName, opt => opt.MapFrom(src => src.RoomStatus.Name))
				.ReverseMap();
			CreateMap<CreateRoomRequest, FAC_Room>();
			CreateMap<UpdateRoomRequest, FAC_Room>();
		}
	}
}



