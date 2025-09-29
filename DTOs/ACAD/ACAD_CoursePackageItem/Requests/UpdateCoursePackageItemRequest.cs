using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_CoursePackageItem.Requests
{
    public class UpdateCoursePackageItemRequest
    {
        public Guid Id { get; set; }
        public Guid PackageID { get; set; }
        public Guid CourseID { get; set; }
        public int Sequence { get; set; }
    }
}
