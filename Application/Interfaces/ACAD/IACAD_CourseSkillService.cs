using DTOs.ACAD.ACAD_CourseSkill.Requests;
using DTOs.ACAD.ACAD_CourseSkill.Responses;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_CourseSkillService
    {
        Task<Guid> CreateCourseSkillAsync(CreateSkillRequest request);
        Task UpdateCourseSkillAsync(Guid id, UpdateCourseSkillRequest request);
        Task DeleteCourseSkillAsync(Guid id);

        Task<CourseSkillResponse?> GetCourseSkillByIdAsync(Guid id);
        Task<IEnumerable<CourseSkillResponse>> GetAllCourseSkillsAsync();
        Task<IEnumerable<CourseSkillResponse>> GetCourseSkillsByCourseAsync(Guid courseId);
        Task<IEnumerable<CourseSkillResponse>> GetCourseSkillsBySkillAsync(Guid skillId);
        Task<CourseSkillResponse?> GetCourseSkillByCourseAndSkillAsync(Guid courseId, Guid skillId);
    }
}
