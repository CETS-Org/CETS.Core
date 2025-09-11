using System;

namespace DTOs.ACAD.ACAD_CourseRequirement.Responses
{
    public class CourseRequirementResponse
    {
        public Guid Id { get; set; }
        public Guid CourseID { get; set; }
        public string CourseName { get; set; } = null!;
        public Guid RequirementID { get; set; }
        public string RequirementName { get; set; } = null!;
    }
}
