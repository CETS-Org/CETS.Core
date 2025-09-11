using DTOs.ACAD.ACAD_CourseBenefit.Requests;
using DTOs.ACAD.ACAD_CourseBenefit.Responses;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_CourseBenefitService
    {
        Task<CourseBenefitResponse> CreateCourseBenefitAsync(CreateCourseBenefitRequest request);
        Task<CourseBenefitResponse> UpdateCourseBenefitAsync(Guid id, UpdateCourseBenefitRequest request);
        Task DeleteCourseBenefitAsync(Guid id);
        Task<CourseBenefitResponse?> GetCourseBenefitByIdAsync(Guid id);
        Task<IEnumerable<CourseBenefitResponse>> GetBenefitsByCourseIdAsync(Guid courseId);
        Task<IEnumerable<CourseBenefitResponse>> GetAllCourseBenefitsAsync();
    }
}
