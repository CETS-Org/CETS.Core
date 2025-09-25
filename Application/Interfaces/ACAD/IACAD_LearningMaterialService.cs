using DTOs.ACAD.ACAD_LearningMaterial.Requests;
using DTOs.ACAD.ACAD_LearningMaterial.Responses;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_LearningMaterialService
    {
        Task<LearningMaterialUploadResponse> CreateLearningMaterialAsync(CreateLearningMaterialRequest request);
        Task UpdateLearningMaterialAsync(UpdateLearningMaterialRequest request);
        Task DeleteLearningMaterialAsync(Guid id);
        Task<LearningMaterialResponse?> GetLearningMaterialByIdAsync(Guid id);
        Task<IEnumerable<LearningMaterialResponse>> GetLearningMaterialsByClassAsync(Guid classId);
        Task<IEnumerable<LearningMaterialResponse>> GetLearningMaterialsByUploaderAsync(Guid uploaderId);
        Task<string> GetDownloadUrlAsync(Guid id);
        Task<string> GetTestPresignedUrlAsync();
    }
}
