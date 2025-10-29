using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_CoursePackage.Responses
{
    public class CoursePackageDetailResponse
    {
        public Guid Id { get; set; }
        public string PackageCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? PackageImageUrl { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalIndividualPrice { get; set; }
        public bool IsActive { get; set; }
        public List<CourseInPackageResponse> Courses { get; set; } = new();
        
        // Audit fields
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }

}
