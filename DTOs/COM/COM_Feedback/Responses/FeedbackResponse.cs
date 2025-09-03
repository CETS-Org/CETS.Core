using System;

namespace DTOs.COM.COM_Feedback.Responses
{
	public class FeedbackResponse
	{
		public Guid Id { get; set; }
		public Guid SubmitterID { get; set; }
		public Guid? FeedbackTypeID { get; set; }
		public Guid? CourseID { get; set; }
		public Guid? TeacherID { get; set; }
		public int? Rating { get; set; }
		public string Comment { get; set; } = null!;
		public DateTime CreatedAt { get; set; }
		public bool IsDeleted { get; set; }
	}
}



