using Domain.Entities;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_CourseSkillRepository : IBaseRepository<ACAD_CourseSkill>
    {
        Task<IEnumerable<ACAD_CourseSkill>> GetByCourseAsync(Guid courseId);
        Task<IEnumerable<ACAD_CourseSkill>> GetBySkillAsync(Guid skillId);
        Task<ACAD_CourseSkill?> GetByCourseAndSkillAsync(Guid courseId, Guid skillId);
    }
}
