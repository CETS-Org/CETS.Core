using Domain.Entities;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_CourseBenefitRepository : IBaseRepository<ACAD_CourseBenefit>
    {
        Task<IEnumerable<ACAD_CourseBenefit>> GetBenefitsByCourseIdAsync(Guid courseId);
        Task<ACAD_CourseBenefit?> GetCourseBenefitAsync(Guid courseId, Guid benefitId);
        Task<bool> ExistsAsync(Guid courseId, Guid benefitId);
    }
}
