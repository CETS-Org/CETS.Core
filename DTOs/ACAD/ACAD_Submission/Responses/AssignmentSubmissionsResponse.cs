namespace DTOs.ACAD.ACAD_Submission.Responses
{
    public class AssignmentSubmissionsResponse
    {
        public AssignmentInfo AssignmentInfo { get; set; } = null!;
        public List<SubmissionDownloadInfo> DownloadUrls { get; set; } = new();
    }

    public class AssignmentInfo
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
