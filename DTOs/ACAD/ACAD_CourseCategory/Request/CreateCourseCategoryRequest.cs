using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_CourseCategory.Request
{
    public class CreateCourseCategoryRequest
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
