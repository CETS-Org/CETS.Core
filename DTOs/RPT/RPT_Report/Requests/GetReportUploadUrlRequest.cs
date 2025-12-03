namespace DTOs.RPT.RPT_Report.Requests
{
	/// <summary>
	/// Request DTO for getting presigned upload URL for report image
	/// </summary>
	public class GetReportUploadUrlRequest
	{
		public string FileName { get; set; } = null!;
		public string ContentType { get; set; } = null!;
	}
}

