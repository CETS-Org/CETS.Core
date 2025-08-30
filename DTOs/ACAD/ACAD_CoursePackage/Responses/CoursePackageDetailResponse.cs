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
        public decimal TotalPrice { get; set; }

        public List<CourseInPackageResponse> Courses { get; set; } = new();
    }

}
