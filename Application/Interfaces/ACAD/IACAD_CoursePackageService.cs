using DTOs.ACAD.ACAD_CoursePackage.Requests;
using DTOs.ACAD.ACAD_CoursePackage.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_CoursePackageService
    {
        Task<Guid> CreatePackageAsync(CreateCoursePackageRequest request);
        Task AddCourseToPackageAsync(AddCourseToPackageRequest request);
        Task<IEnumerable<CoursePackageResponse>> GetActivePackagesAsync();
        Task<CoursePackageDetailResponse?> GetPackageDetailAsync(Guid packageId);
    }
}
