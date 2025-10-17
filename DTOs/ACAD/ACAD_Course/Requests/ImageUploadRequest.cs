using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Course.Requests
{
    public class ImageUploadRequest
    {
        public string FileName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
    }
}

