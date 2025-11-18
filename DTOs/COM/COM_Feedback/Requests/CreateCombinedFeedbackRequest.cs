using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.COM.COM_Feedback.Requests
{
	public class CreateCombinedFeedbackRequest
	{
		[Required]
		public Guid SubmitterID { get; set; }

		[Required]
		public Guid CourseID { get; set; }

		[Required]
		public Guid TeacherID { get; set; }

		// Course Feedback
		public CourseFeedbackData? CourseFeedback { get; set; }

		// Teacher Feedback
		public TeacherFeedbackData? TeacherFeedback { get; set; }
	}

	public class CourseFeedbackData
	{
		[Range(1, 5)]
		public int? Rating { get; set; }

		[StringLength(4000)]
		public string? Comment { get; set; }

		[StringLength(50)]
		public string? ContentClarity { get; set; }

		[StringLength(50)]
		public string? CourseRelevance { get; set; }

		[StringLength(50)]
		public string? MaterialsQuality { get; set; }
	}

	public class TeacherFeedbackData
	{
		[Range(1, 5)]
		public int? Rating { get; set; }

		[StringLength(4000)]
		public string? Comment { get; set; }

		[StringLength(50)]
		public string? TeachingEffectiveness { get; set; }

		[StringLength(50)]
		public string? CommunicationSkills { get; set; }

		[StringLength(50)]
		public string? TeacherSupportiveness { get; set; }
	}
}
