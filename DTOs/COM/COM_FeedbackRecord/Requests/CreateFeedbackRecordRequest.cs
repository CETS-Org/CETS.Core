using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.COM.COM_FeedbackRecord.Requests
{
	public class CreateFeedbackRecordRequest
	{
		[StringLength(2048)]
		public string? FormUrl { get; set; }

		[StringLength(2048)]
		public string? ResultUrl { get; set; }

		[Required]
		public Guid CreatedBy { get; set; }
	}
}



