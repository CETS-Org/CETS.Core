using System;

namespace DTOs.COM.COM_Conversation.Responses
{
	public class ConversationResponse
	{
		public Guid Id { get; set; }
		public Guid SenderID { get; set; }
		public Guid RecipientID { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}



