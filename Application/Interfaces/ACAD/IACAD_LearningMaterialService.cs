using DTOs.ACAD.ACAD_LearningMaterial.Requests;
using DTOs.ACAD.ACAD_LearningMaterial.Responses;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_LearningMaterialService
    {
        Task<LearningMaterialUploadResponse> CreateLearningMaterialAsync(CreateLearningMaterialRequest request);
        Task<LearningMaterialUploadResponse?> UpdateLearningMaterialAsync(UpdateLearningMaterialRequest request);
        Task DeleteLearningMaterialAsync(Guid id);
        Task<LearningMaterialResponse?> GetLearningMaterialByIdAsync(Guid id);
        Task<IEnumerable<LearningMaterialResponse>> GetLearningMaterialsByClassMeetingAsync(Guid classMeetingId);
        Task<IEnumerable<LearningMaterialResponse>> GetLearningMaterialsByUploaderAsync(Guid uploaderId);
        Task<string> GetDownloadUrlAsync(Guid id);
    }
}
