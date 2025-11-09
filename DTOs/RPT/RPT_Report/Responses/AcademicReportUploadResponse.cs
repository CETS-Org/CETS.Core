namespace DTOs.RPT.RPT_Report.Responses
{
	/// <summary>
	/// Response DTO for submitting academic request with file upload support
	/// </summary>
	public class AcademicReportUploadResponse
	{
		public AcademicReportResponse Report { get; set; } = null!;
		public string? UploadUrl { get; set; }
		public string? FilePath { get; set; }
	}
}

