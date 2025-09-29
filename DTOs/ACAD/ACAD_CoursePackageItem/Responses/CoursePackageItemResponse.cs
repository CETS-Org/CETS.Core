using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_CoursePackageItem.Responses
{
    public class CoursePackageItemResponse
    {
        public Guid Id { get; set; }
        public Guid PackageID { get; set; }
        public Guid CourseID { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public int Sequence { get; set; }
        public bool IsDeleted { get; set; }
    }
}
