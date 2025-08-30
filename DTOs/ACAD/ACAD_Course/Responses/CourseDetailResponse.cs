using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Course.Responses
{
    public class CourseDetailResponse : CourseResponse
    {
        public decimal StandardPrice { get; set; }
        public string CategoryName { get; set; } = null!;
        public string LevelName { get; set; } = null!;
        public string FormatName { get; set; } = null!;
    }
}
