using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.RPT.RPT_Report.Requests
{
	
	public class SubmitAcademicReportRequest
	{
		
		[Required(ErrorMessage = "Report Type is required")]
		public Guid ReportTypeID { get; set; }

		[Required(ErrorMessage = "Submitter ID is required")]
		public Guid SubmittedBy { get; set; }

		[Required(ErrorMessage = "Title is required")]
		[StringLength(255, ErrorMessage = "Title cannot exceed 255 characters")]
		public string Title { get; set; } = null!;

		[Required(ErrorMessage = "Description is required")]
		[StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
		public string Description { get; set; } = null!;

		
		public string? FileName { get; set; }
		public string? ContentType { get; set; }

		[StringLength(2048)]
		public string? AttachmentUrl { get; set; }
	}
}


