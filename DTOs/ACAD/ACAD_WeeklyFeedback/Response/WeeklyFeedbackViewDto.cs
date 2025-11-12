using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_WeeklyFeedback.Response
{
    public record WeeklyFeedbackViewDto
    {
        public Guid Id { get; init; }
        public Guid ClassID { get; init; }
        public Guid? ClassMeetingId { get; init; }
        public Guid TeacherId { get; init; }
        public Guid StudentId { get; init; }
        public int WeekNumber { get; init; }
        public string Participation { get; init; } = "";
        public string AssignmentQuality { get; init; } = "";
        public string SkillProgress { get; init; } = "";
        public string? NextStep { get; init; }
        public string? CustomNote { get; init; }
        public int Status { get; init; }
        public DateTime UpdatedAt { get; init; }

        // enrich (optional)
        public string? StudentName { get; init; }
        public string? TeacherName { get; init; }
        public string? ClassName { get; init; }
    }
}
