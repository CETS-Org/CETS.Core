using Domain.Entities;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_CoursePackageItemRepository : IBaseRepository<ACAD_CoursePackageItem>
    {
        Task<IEnumerable<ACAD_CoursePackageItem>> GetByPackageIdAsync(Guid packageId);
    }
}


