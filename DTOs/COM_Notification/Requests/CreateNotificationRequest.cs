using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.COM_Notification.Requests
{
	public class CreateNotificationRequest
	{
		[Required]
		[StringLength(4000)]
		public string Content { get; set; } = null!;

		public bool IsPush { get; set; }
	}
}



