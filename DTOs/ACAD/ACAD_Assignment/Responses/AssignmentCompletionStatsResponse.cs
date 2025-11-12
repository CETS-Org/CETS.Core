using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Assignment.Responses
{
    public class AssignmentCompletionStatsResponse
    {
        public int TotalAssignments { get; set; }
        public int CompletedOnTime { get; set; }
        public int CompletedLate { get; set; }
        public int PendingGrading { get; set; }
        public int NotSubmitted { get; set; }

        public decimal CompletionRate => TotalAssignments == 0
            ? 0
            : Math.Round(((decimal)(CompletedOnTime + CompletedLate) / TotalAssignments) * 100, 2);
    }
}
