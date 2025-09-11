using System;

namespace DTOs.ACAD.ACAD_CourseRequirement.Requests
{
    public class CreateCourseRequirementRequest
    {
        public Guid CourseID { get; set; }
        public Guid RequirementID { get; set; }
    }
}
