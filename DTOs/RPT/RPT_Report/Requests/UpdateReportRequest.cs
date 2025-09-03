using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.RPT.RPT_Report.Requests
{
	public class UpdateReportRequest
	{
		[Required]
		public Guid ReportTypeID { get; set; }

		[Required]
		public Guid SubmittedBy { get; set; }

		[Required]
		[StringLength(255)]
		public string Title { get; set; } = null!;

		[Required]
		public string Description { get; set; } = null!;

		[StringLength(2048)]
		public string? AttachmentUrl { get; set; }

		[Required]
		public Guid ReportStatusID { get; set; }

		[StringLength(2048)]
		public string? ReportUrl { get; set; }

		public DateTime? ResolvedAt { get; set; }
		public Guid? ResolvedBy { get; set; }
	}
}



