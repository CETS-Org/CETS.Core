using Domain.Entities;
using DTOs.ACAD.ACAD_Class.Responses;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_ClassRepository : IBaseRepository<ACAD_Class>
    {
        Task<List<LearningClassResponse>> GetLearningClassByStudentId (Guid classId);
    }

}


