using System;

namespace DTOs.COM_Notification.Responses
{
	public class NotificationResponse
	{
		public Guid Id { get; set; }
		public string Content { get; set; } = null!;
		public DateTime CreatedAt { get; set; }
		public bool IsPush { get; set; }
	}
}



