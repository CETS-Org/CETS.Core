using DTOs.ACAD.ACAD_CoursePackage.Requests;
using DTOs.ACAD.ACAD_CoursePackage.Responses;
using DTOs.ACAD.ACAD_CoursePackage.Search;
using DTOs.ACAD.ACAD_CoursePackageItem.Requests;
using DTOs.ACAD.ACAD_CoursePackageItem.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ACAD
{
    public interface IACAD_CoursePackageService
    {
        // CoursePackage operations
        Task<Guid> CreatePackageAsync(CreateCoursePackageRequest request);
        Task UpdatePackageAsync(UpdateCoursePackageRequest request);
        Task SoftDeletePackageAsync(Guid packageId);
        Task<IEnumerable<CoursePackageResponse>> GetAllPackagesAsync();
        Task<IEnumerable<CoursePackageResponse>> GetActivePackagesAsync();
        Task<CoursePackageResponse?> GetPackageByIdAsync(Guid packageId);
        Task<CoursePackageDetailResponse?> GetPackageDetailAsync(Guid packageId);
        Task ActivatePackageAsync(Guid packageId);
        Task DeactivatePackageAsync(Guid packageId);
        
        // CoursePackageItem operations
        Task AddCourseToPackageAsync(Guid packageId, AddCourseToPackageRequest request);
        Task RemoveCourseFromPackageAsync(RemoveCourseFromPackageRequest request);
        Task<IEnumerable<CoursePackageItemResponse>> GetPackageItemsAsync(Guid packageId);
        Task UpdatePackageItemSequenceAsync(Guid packageItemId, int newSequence);
        
        // Search operations
        Task<CoursePackageSearchResult> SearchBasicAsync(CoursePackageSearchQuery query, CancellationToken ct);
        
        // Statistics operations
        Task<CoursePackageStatisticsResponse> GetStatisticsAsync();
        
        // Image upload operations
        Task<CoursePackageImageUploadResponse> GetImageUploadUrlAsync(string fileName, string contentType);
    }
}
