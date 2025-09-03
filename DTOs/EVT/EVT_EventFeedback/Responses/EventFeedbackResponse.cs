using System;

namespace DTOs.EVT.EVT_EventFeedback.Responses
{
	public class EventFeedbackResponse
	{
		public Guid Id { get; set; }
		public Guid EventID { get; set; }
		public Guid AccountID { get; set; }
		public int? Rating { get; set; }
		public string? Comment { get; set; }
		public string? FeedbackUrl { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}



