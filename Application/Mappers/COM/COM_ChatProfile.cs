using AutoMapper;
using Domain.Entities.MongoDB;
using DTOs.COM.COM_Chat.Requests;
using DTOs.COM.COM_Chat.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.COM
{
    public class COM_ChatProfile : Profile
    {
        public COM_ChatProfile()
        {
            // Room Mappings
            CreateMap<CreateChatRoomRequest, COM_ChatRoom>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.LastMessageAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<COM_ChatRoom, ChatRoomResponse>();

            // Message Mappings
            CreateMap<SendMessageRequest, COM_ChatMessage>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<COM_ChatMessage, ChatMessageResponse>();
        }
    }
}
