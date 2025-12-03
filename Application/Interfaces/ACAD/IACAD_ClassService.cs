using DTOs.ACAD.ACAD_Class.Requests;
using DTOs.ACAD.ACAD_Class.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_ClassService
    {
        Task<Guid> CreateClassAsync(CreateClassRequest request);
        Task UpdateClassAsync(UpdateClassRequest request);
        Task DeleteClassAsync(Guid id);

        Task<ClassResponse?> GetClassByIdAsync(Guid id);
        Task<IEnumerable<ClassResponse>> GetAllClassesAsync();
        Task<IEnumerable<ClassResponse>> GetClassesByCourseIdAsync(Guid courseId);
        Task<IEnumerable<ClassResponse>> GetClassesByCourseIdAsync2(Guid courseId);
        Task<List<LearningClassResponse>> GetLearningClassByStudentId(Guid studentId);

        Task<ClassDetailResponse?> GetClassDetailAsync(Guid classId);
        Task<List<ClassRowResponse>> GetAllClassRowsAsync();
        Task<ClassStaffViewResponse?> GetClassByIdStaffView(Guid id);
        Task<List<ClassStaffViewResponse>> GetClassByCourseStaffView(Guid courseId);

        Task<Guid> CreateClassWithScheduleAsync(CreateClassWithScheduleRequest request);

        Task SoftDeleteClassAsync(Guid id);

        Task<List<FeedbackClassResponse>> GetFeedbackClassesByStudentId(Guid studentId);

        Task<ClassDetailForEditResponse> GetClassDetailForEditAsync(Guid classId);
        Task UpdateClassCompositeAsync(Guid classId, UpdateClassCompositeRequest request);

    }
}
