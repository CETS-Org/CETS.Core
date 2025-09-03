using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.COM_Feedback.Requests
{
	public class CreateFeedbackRequest
	{
		[Required]
		public Guid SubmitterID { get; set; }

		public Guid? FeedbackTypeID { get; set; }
		public Guid? CourseID { get; set; }
		public Guid? TeacherID { get; set; }

		[Range(1, 5)]
		public int? Rating { get; set; }

		[Required]
		[StringLength(4000)]
		public string Comment { get; set; } = null!;
	}
}



