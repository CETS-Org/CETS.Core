using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_WeeklyFeedback.Request
{
    public record UpsertWeeklyFeedbackRequestDto
    {
        public Guid ClassID { get; init; }
        public Guid? ClassMeetingId { get; init; }
        public Guid? TeacherId { get; init; }
        public int WeekNumber { get; init; }
        public bool Submit { get; init; }
        public List<UpsertWeeklyFeedbackItemDto> Items { get; init; } = new();
    }
}
