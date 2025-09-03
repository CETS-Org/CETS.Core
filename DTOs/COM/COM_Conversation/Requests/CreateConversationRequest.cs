using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.COM.COM_Conversation.Requests
{
	public class CreateConversationRequest
	{
		[Required]
		public Guid SenderID { get; set; }

		[Required]
		public Guid RecipientID { get; set; }
	}
}



