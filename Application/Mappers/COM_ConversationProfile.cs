using AutoMapper;
using Domain.Entities;
using DTOs.COM_Conversation.Requests;
using DTOs.COM_Conversation.Responses;

namespace Application.Mappers
{
	public class COM_ConversationProfile : Profile
	{
		public COM_ConversationProfile()
		{
			CreateMap<COM_Conversation, ConversationResponse>().ReverseMap();
			CreateMap<CreateConversationRequest, COM_Conversation>();
			CreateMap<UpdateConversationRequest, COM_Conversation>();
		}
	}
}



