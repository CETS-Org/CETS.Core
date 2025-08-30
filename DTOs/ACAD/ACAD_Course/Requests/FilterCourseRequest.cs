using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Course.Requests
{
    public class FilterCourseRequest
    {
        public Guid? LevelId { get; set; }
        public Guid? FormatId { get; set; }
        public Guid? TeacherId { get; set; }
        public string? Keyword { get; set; }
    }
}
