using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.RPT.RPT_Report.Requests
{
	public class ProcessAcademicReportRequest
	{
	
		[Required(ErrorMessage = "Processed by staff ID is required")]
		public Guid ProcessedBy { get; set; }

	
		[Required(ErrorMessage = "New status ID is required")]
		public Guid NewStatusId { get; set; }

		[StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
		public string? Notes { get; set; }
	}
}




