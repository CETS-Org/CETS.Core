using DTOs.ACAD.ACAD_Course.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Enrollment.Responses
{
    public class AcademicResultResponse
    {
        public int TotalCourses { get; set; }
        public int PassedCourses { get; set; }
        public int FailedCourses { get; set; }
        public int InProgressCourses { get; set; }
        public IReadOnlyList<CourseItemResponse> Items { get; set; } = Array.Empty<CourseItemResponse>();
    }
}
