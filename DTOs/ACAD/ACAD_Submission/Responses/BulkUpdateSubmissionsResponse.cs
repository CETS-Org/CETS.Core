namespace DTOs.ACAD.ACAD_Submission.Responses
{
    public class BulkUpdateSubmissionsResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public BulkUpdateData Data { get; set; } = new BulkUpdateData();
    }
}



