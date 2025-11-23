namespace DTOs.ACAD.ACAD_AcademicRequest.Requests
{
    public class GetUploadUrlRequest
    {
        public string FileName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
    }
}

