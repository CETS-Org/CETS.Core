using System;

namespace DTOs.ACAD.ACAD_CourseBenefit.Requests
{
    public class CreateCourseBenefitRequest
    {
        public Guid CourseID { get; set; }
        public Guid BenefitID { get; set; }
    }
}
