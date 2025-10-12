namespace DTOs.ACAD.ACAD_Submission.Responses
{
    public class SubmissionDownloadInfo
    {
        public Guid SubmissionId { get; set; }
        public string StudentCode { get; set; } = null!;
        public string StudentName { get; set; } = null!;
        public string DownloadUrl { get; set; } = null!;
        public string FileName { get; set; } = null!;
    }
}
