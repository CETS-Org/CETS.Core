using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Enrollment.Responses
{
    public class EnrollmentDetailResponse : EnrollmentResponse
    {
        public string? StudentName { get; set; }
        public string? CourseName { get; set; }
        public string? ClassName { get; set; }
        public string? StatusName { get; set; }
    }
}
