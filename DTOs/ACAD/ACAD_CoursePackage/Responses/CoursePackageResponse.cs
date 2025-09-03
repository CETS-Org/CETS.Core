using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_CoursePackage.Responses
{
    public class CoursePackageResponse
    {
        public Guid Id { get; set; }
        public string PackageCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
    }
}
