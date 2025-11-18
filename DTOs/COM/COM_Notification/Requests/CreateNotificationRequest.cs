using System.ComponentModel.DataAnnotations;

namespace DTOs.COM.COM_Notification.Requests
{
	public class CreateNotificationRequest
	{
		[Required]
		[StringLength(64, MinimumLength = 1)]
		public string UserId { get; set; } = null!;

		[Required]
		[StringLength(200)]
		public string Title { get; set; } = null!;

		[Required]
		[StringLength(4000)]
		public string Message { get; set; } = null!;

		[Required]
		[RegularExpression("^(info|warning|system|chat)$", ErrorMessage = "Type must be one of info, warning, system, chat")]
		public string Type { get; set; } = "info";

		public bool IsRead { get; set; }
	}
}



