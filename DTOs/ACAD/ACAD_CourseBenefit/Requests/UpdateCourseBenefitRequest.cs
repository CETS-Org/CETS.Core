using System;

namespace DTOs.ACAD.ACAD_CourseBenefit.Requests
{
    public class UpdateCourseBenefitRequest
    {
        public Guid CourseID { get; set; }
        public Guid BenefitID { get; set; }
    }
}
