using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Enrollment.Requests
{
    public class CreateEnrollmentRequest
    {
        public Guid StudentID { get; set; }
        public Guid CourseID { get; set; }
        public Guid? ClassID { get; set; }
    }
}
