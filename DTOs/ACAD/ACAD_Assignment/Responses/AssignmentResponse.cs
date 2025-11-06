using DTOs.ACAD.ACAD_Submission.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Assignment.Responses
{
    public class AssignmentResponse
    {
        public Guid Id { get; set; }
        public Guid ClassMeetingId { get; set; }
        public Guid TeacherId { get; set; }
        public string Title { get; set; } = null!;
        public string FileUrl { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<SubmissionResponse>? Submissions { get; set; }
     
        public Guid? SkillID { get; set; }
        public string? SkillName { get; set; }
    }
}
