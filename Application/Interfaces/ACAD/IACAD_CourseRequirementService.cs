using DTOs.ACAD.ACAD_CourseRequirement.Requests;
using DTOs.ACAD.ACAD_CourseRequirement.Responses;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_CourseRequirementService
    {
        Task<CourseRequirementResponse> CreateCourseRequirementAsync(CreateCourseRequirementRequest request);
        Task<CourseRequirementResponse> UpdateCourseRequirementAsync(Guid id, UpdateCourseRequirementRequest request);
        Task DeleteCourseRequirementAsync(Guid id);
        Task<CourseRequirementResponse?> GetCourseRequirementByIdAsync(Guid id);
        Task<IEnumerable<CourseRequirementResponse>> GetRequirementsByCourseIdAsync(Guid courseId);
        Task<IEnumerable<CourseRequirementResponse>> GetAllCourseRequirementsAsync();
    }
}
