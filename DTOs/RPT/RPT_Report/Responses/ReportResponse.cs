using System;

namespace DTOs.RPT.RPT_Report.Responses
{
	public class ReportResponse
	{
		public Guid Id { get; set; }
		public Guid ReportTypeID { get; set; }
		public Guid SubmittedBy { get; set; }
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string? AttachmentUrl { get; set; }
		public Guid ReportStatusID { get; set; }
		public string? ReportUrl { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? ResolvedAt { get; set; }
		public Guid? ResolvedBy { get; set; }
	}
}



