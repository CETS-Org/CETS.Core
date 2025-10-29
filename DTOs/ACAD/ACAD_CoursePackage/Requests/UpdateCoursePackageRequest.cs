using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_CoursePackage.Requests
{
    public class UpdateCoursePackageRequest
    {
        public Guid Id { get; set; }
        public string PackageCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal TotalPrice { get; set; }
        public string? PackageImageUrl { get; set; }
        public bool IsActive { get; set; }
        public List<Guid>? CourseIDs { get; set; }
    }
}
