using System;
using System.Collections.Generic;

namespace DTOs.ACAD.ACAD_Enrollment.Responses
{
    public class BulkUpdateFinalGradesResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public BulkUpdateFinalGradesData Data { get; set; } = new BulkUpdateFinalGradesData();
    }

    public class BulkUpdateFinalGradesData
    {
        public int UpdatedCount { get; set; }
        public int FailedCount { get; set; }
        public List<FinalGradeUpdateResult> Results { get; set; } = new List<FinalGradeUpdateResult>();
    }

    public class FinalGradeUpdateResult
    {
        public Guid EnrollmentId { get; set; }
        public string Status { get; set; } = string.Empty; // "success" or "failed"
        public string? Error { get; set; }
    }
}


