using Domain.Entities;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_SyllabusRepository : IBaseRepository<ACAD_Syllabus>
    {
        Task<IEnumerable<ACAD_Syllabus>> GetByCourseIdAsync(Guid courseId);
    }
}


