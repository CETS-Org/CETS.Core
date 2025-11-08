using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Assignment.Requests
{
    public class CreateAssignmentRequest
    {
        public Guid ClassMeetingId { get; set; }
        public Guid TeacherId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public string AssignmentType { get; set; } = "homework"; // "quiz" or "homework"
    }
}
