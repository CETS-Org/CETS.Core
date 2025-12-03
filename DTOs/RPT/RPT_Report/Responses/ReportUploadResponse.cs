namespace DTOs.RPT.RPT_Report.Responses
{
	/// <summary>
	/// Response DTO for getting presigned upload URL for report image
	/// </summary>
	public class ReportUploadResponse
	{
		public string UploadUrl { get; set; } = null!;
		public string FilePath { get; set; } = null!;
	}
}

