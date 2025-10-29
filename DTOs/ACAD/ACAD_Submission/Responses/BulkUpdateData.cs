using System.Collections.Generic;

namespace DTOs.ACAD.ACAD_Submission.Responses
{
    public class BulkUpdateData
    {
        public int UpdatedCount { get; set; }
        public int FailedCount { get; set; }
        public List<SubmissionUpdateResult> Results { get; set; } = new List<SubmissionUpdateResult>();
    }
}


