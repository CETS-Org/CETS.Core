using Application.Interfaces.COM;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.COM;
using DTOs.COM_Conversation.Requests;
using DTOs.COM_Conversation.Responses;

namespace Application.Implementations.COM
{
	public class COM_ConversationService : BaseService<COM_Conversation, ConversationResponse, UpdateConversationRequest, CreateConversationRequest>, ICOM_ConversationService
	{
		public COM_ConversationService(ICOM_ConversationRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
			: base(repository, unitOfWork, mapper)
		{
		}
	}
}



