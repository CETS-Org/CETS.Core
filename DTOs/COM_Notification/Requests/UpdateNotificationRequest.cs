using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.COM_Notification.Requests
{
	public class UpdateNotificationRequest
	{
		[Required]
		[StringLength(4000)]
		public string Content { get; set; } = null!;

		public bool IsPush { get; set; }
	}
}



