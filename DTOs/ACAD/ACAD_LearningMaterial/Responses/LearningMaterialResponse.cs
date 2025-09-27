namespace DTOs.ACAD.ACAD_LearningMaterial.Responses
{
    public class LearningMaterialResponse
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? ClassID { get; set; }
        public string Title { get; set; } = null!;
        public string? StoreUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UploaderName { get; set; }
        public string? ClassName { get; set; }
    }
}
