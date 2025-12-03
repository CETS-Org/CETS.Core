using System;

namespace DTOs.RPT.RPT_Report.Responses
{
	public class ReportStatusInfo
	{
		public Guid Id { get; set; }
		public string Code { get; set; } = null!;
		public string Name { get; set; } = null!;
		public bool IsActive { get; set; }
	}

	public class ReportResponse
	{
		public Guid Id { get; set; }
		public Guid ReportTypeID { get; set; }
		public string? ReportTypeName { get; set; }
		public Guid SubmittedBy { get; set; }
		public string? SubmitterName { get; set; }
		public string? SubmitterEmail { get; set; }
		public string? SubmitterRole { get; set; }
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string? AttachmentUrl { get; set; }
		public Guid ReportStatusID { get; set; }
		public string? StatusName { get; set; }
		public ReportStatusInfo? ReportStatus { get; set; }
		public string? Priority { get; set; }
		public string? ReportUrl { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? ResolvedAt { get; set; }
		public Guid? ResolvedBy { get; set; }
		public string? ResolvedByName { get; set; }
		public string? AdminResponse { get; set; }
	}
}



