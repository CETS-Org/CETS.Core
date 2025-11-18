using Domain.Entities;
using DTOs.ACAD.ACAD_Class.Responses;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_ClassRepository : IBaseRepository<ACAD_Class>
    {
        Task<List<LearningClassResponse>> GetLearningClassByStudentId(Guid studentId);
        Task<ClassDetailResponse?> GetClassDetailAsync(Guid classId);
        Task<List<ClassResponse>> GetClassesByCourseIdAsync(Guid courseId);
        Task<List<ClassRowResponse>> GetAllClassRowsAsync();
        Task<List<ACAD_Class>> GetAllClassStaffView();
        Task<ACAD_Class?> GetClassStaffViewById(Guid id);
        Task<List<FeedbackClassResponse>> GetFeedbackClassesByStudentId(Guid studentId);
    }
}
