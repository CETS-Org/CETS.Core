using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Enrollment.Responses
{
    public class CourseEnrollmentListResponse
    {
        public Guid Id { get; set; } // EnrollmentID
        public Guid CourseId { get; set; } // CourseID
        public string CourseCode { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public string? Description { get; set; }
        public string? CourseImageUrl { get; set; }
        public bool IsActive { get; set; }
        public List<string> Teachers { get; set; } = new();
        public string? EnrollmentStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? TentativeStartDate { get; set; }
        public string? ClassName { get; set; }

    }
}
