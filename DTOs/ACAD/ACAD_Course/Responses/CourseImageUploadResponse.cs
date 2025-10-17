using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Course.Responses
{
    public class CourseImageUploadResponse
    {
        public string UploadUrl { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string PublicUrl { get; set; } = null!;
    }
}

