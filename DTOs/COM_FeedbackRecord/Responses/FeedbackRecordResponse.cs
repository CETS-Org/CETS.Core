using System;

namespace DTOs.COM_FeedbackRecord.Responses
{
	public class FeedbackRecordResponse
	{
		public Guid Id { get; set; }
		public string? FormUrl { get; set; }
		public string? ResultUrl { get; set; }
		public DateTime CreatedAt { get; set; }
		public Guid CreatedBy { get; set; }
		public bool IsDeleted { get; set; }
	}
}



