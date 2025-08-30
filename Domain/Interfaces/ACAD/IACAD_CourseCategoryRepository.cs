using Domain.Entities;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_CourseCategoryRepository : IBaseRepository<ACAD_CourseCategory>
    {
        Task<ACAD_CourseCategory?> GetByCodeAsync(string code);
    }
}


