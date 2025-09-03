using AutoMapper;
using Domain.Entities;
using DTOs.COM.COM_Conversation.Requests;
using DTOs.COM.COM_Conversation.Responses;

namespace Application.Mappers.COM
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



