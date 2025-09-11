using System;

namespace DTOs.ACAD.ACAD_CourseBenefit.Responses
{
    public class CourseBenefitResponse
    {
        public Guid Id { get; set; }
        public Guid CourseID { get; set; }
        public string CourseName { get; set; } = null!;
        public Guid BenefitID { get; set; }
        public string BenefitName { get; set; } = null!;
    }
}
