using Domain.Entities;


namespace Domain.Interfaces.ACAD
{
    public interface IACAD_CourseRepository : IBaseRepository<ACAD_Course>
    {
        Task<IEnumerable<ACAD_Course>> SearchAsync(string keyword);
        Task<IEnumerable<ACAD_Course>> FilterAsync(Guid? levelId, Guid? formatId, Guid? teacherId);
        Task<ACAD_Course?> GetDetailAsync(Guid courseId);
        IQueryable<ACAD_Course> GetAllCoursesForListAsync();
    }
}


