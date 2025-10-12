namespace DTOs.ACAD.ACAD_Assignment.Responses
{
    public class AssignmentUploadResponse
    {
        public Guid Id { get; set; }
        public string UploadUrl { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string Title { get; set; } = null!;
        public DateTime DueDate { get; set; }
    }
}



