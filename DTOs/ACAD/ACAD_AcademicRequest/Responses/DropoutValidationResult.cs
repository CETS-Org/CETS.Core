using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_AcademicRequest.Responses
{
    public class DropoutValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
        public bool HasUnpaidInvoices { get; set; }
        public bool HasPendingRequests { get; set; }
        public bool CompletedExitSurvey { get; set; }
    }
}

