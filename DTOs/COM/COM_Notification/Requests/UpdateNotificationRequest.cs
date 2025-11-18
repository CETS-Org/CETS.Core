using System.ComponentModel.DataAnnotations;

namespace DTOs.COM.COM_Notification.Requests
{
	public class UpdateNotificationRequest
	{
		[StringLength(200)]
		public string? Title { get; set; }

		[StringLength(4000)]
		public string? Message { get; set; }

		[RegularExpression("^(info|warning|system|chat)$", ErrorMessage = "Type must be one of info, warning, system, chat")]
		public string? Type { get; set; }

		public bool? IsRead { get; set; }
	}
}



