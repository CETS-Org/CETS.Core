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
		public string? Comment { get; set; }

		// Course Feedback Fields
		public string? ContentClarity { get; set; }
		public string? CourseRelevance { get; set; }
		public string? MaterialsQuality { get; set; }

		// Teacher Feedback Fields
		public string? TeachingEffectiveness { get; set; }
		public string? CommunicationSkills { get; set; }
		public string? TeacherSupportiveness { get; set; }

		public DateTime CreatedAt { get; set; }
		public bool IsDeleted { get; set; }
	}
}



