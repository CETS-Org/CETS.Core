using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Course.Requests
{
    public class UpdateCourseRequest : CreateCourseRequest
    {
        public Guid Id { get; set; }
    }
}
