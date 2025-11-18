using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.COM.COM_Feedback.Requests
{
	public class CreateFeedbackRequest
	{
		[Required]
		public Guid SubmitterID { get; set; }

		[Required]
		public Guid FeedbackTypeID { get; set; }

		public Guid? CourseID { get; set; }
		public Guid? TeacherID { get; set; }

		[Range(1, 5)]
		public int? Rating { get; set; }

		[StringLength(4000)]
		public string? Comment { get; set; }

		// Course Feedback Fields (required when FeedbackType = COURSE_FEEDBACK)
		[StringLength(50)]
		public string? ContentClarity { get; set; }

		[StringLength(50)]
		public string? CourseRelevance { get; set; }

		[StringLength(50)]
		public string? MaterialsQuality { get; set; }

		// Teacher Feedback Fields (required when FeedbackType = TEACHER_FEEDBACK)
		[StringLength(50)]
		public string? TeachingEffectiveness { get; set; }

		[StringLength(50)]
		public string? CommunicationSkills { get; set; }

		[StringLength(50)]
		public string? TeacherSupportiveness { get; set; }
	}
}



