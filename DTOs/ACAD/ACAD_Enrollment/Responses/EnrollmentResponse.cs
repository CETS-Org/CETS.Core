using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Enrollment.Responses
{
    public class EnrollmentResponse
    {
        public Guid Id { get; set; }
        public Guid StudentID { get; set; }
        public Guid CourseID { get; set; }
        public Guid? ClassID { get; set; }
        public Guid EnrollmentStatusID { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public decimal? FinalGrade { get; set; }
    }
}
