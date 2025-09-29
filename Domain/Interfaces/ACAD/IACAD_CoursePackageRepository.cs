using Domain.Entities;
using DTOs.ACAD.ACAD_CoursePackage.Search;

namespace Domain.Interfaces.ACAD
{
    public interface IACAD_CoursePackageRepository : IBaseRepository<ACAD_CoursePackage>
    {
        Task<IEnumerable<ACAD_CoursePackage>> GetActivePackagesAsync();
        Task<ACAD_CoursePackage?> GetDetailAsync(Guid packageId);
        Task<CoursePackageSearchResult> SearchBasicAsync(CoursePackageSearchQuery query, CancellationToken ct);
    }
}


