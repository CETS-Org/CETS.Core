using System;

namespace DTOs.ACAD.ACAD_CourseRequirement.Requests
{
    public class UpdateCourseRequirementRequest
    {
        public Guid CourseID { get; set; }
        public Guid RequirementID { get; set; }
    }
}
