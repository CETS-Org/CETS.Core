using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_WeeklyFeedback.Request
{
    public record UpsertWeeklyFeedbackItemDto
    {
        public Guid StudentId { get; init; }
        public string Participation { get; init; } = "";
        public string AssignmentQuality { get; init; } = "";
        public string SkillProgress { get; init; } = "";
        public string? NextStep { get; init; }
        public string? CustomNote { get; init; }
    }
}
