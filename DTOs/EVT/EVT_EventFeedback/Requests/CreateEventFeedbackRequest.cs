using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.EVT.EVT_EventFeedback.Requests
{
	public class CreateEventFeedbackRequest
	{
		[Required]
		public Guid EventID { get; set; }

		[Required]
		public Guid AccountID { get; set; }

		[Range(1,5)]
		public int? Rating { get; set; }

		[StringLength(4000)]
		public string? Comment { get; set; }

		[StringLength(2048)]
		public string? FeedbackUrl { get; set; }
	}
}



