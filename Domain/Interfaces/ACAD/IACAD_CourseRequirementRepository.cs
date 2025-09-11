using Domain.Entities;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_CourseRequirementRepository : IBaseRepository<ACAD_CourseRequirement>
    {
        Task<IEnumerable<ACAD_CourseRequirement>> GetRequirementsByCourseIdAsync(Guid courseId);
        Task<ACAD_CourseRequirement?> GetCourseRequirementAsync(Guid courseId, Guid requirementId);
        Task<bool> ExistsAsync(Guid courseId, Guid requirementId);
    
    }
}
