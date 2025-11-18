using System;

namespace DTOs.COM.COM_Feedback.Responses
{
	public class CombinedFeedbackResponse
	{
		public FeedbackResponse? CourseFeedback { get; set; }
		public FeedbackResponse? TeacherFeedback { get; set; }
		public bool Success { get; set; }
		public string? Message { get; set; }
	}
}
