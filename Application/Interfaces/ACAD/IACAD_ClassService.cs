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
        Task<List<LearningClassResponse>> GetLearningClassByStudentId(Guid studentId);

    }
}
