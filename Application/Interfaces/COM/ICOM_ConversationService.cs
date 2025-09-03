using Domain.Entities;
using DTOs.COM_Conversation.Requests;
using DTOs.COM_Conversation.Responses;

namespace Application.Interfaces.COM
{
	public interface ICOM_ConversationService : IBaseService<COM_Conversation, ConversationResponse, UpdateConversationRequest, CreateConversationRequest>
	{
	}
}



