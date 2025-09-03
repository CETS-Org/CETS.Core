using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.EVT.EVT_EventFeedback.Requests
{
	public class UpdateEventFeedbackRequest
	{
		[Range(1,5)]
		public int? Rating { get; set; }

		[StringLength(4000)]
		public string? Comment { get; set; }

		[StringLength(2048)]
		public string? FeedbackUrl { get; set; }
	}
}



