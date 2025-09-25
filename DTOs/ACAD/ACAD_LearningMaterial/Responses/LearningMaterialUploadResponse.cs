namespace DTOs.ACAD.ACAD_LearningMaterial.Responses
{
    public class LearningMaterialUploadResponse
    {
        public Guid Id { get; set; }
        public string UploadUrl { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string Title { get; set; } = null!;
    }
}
