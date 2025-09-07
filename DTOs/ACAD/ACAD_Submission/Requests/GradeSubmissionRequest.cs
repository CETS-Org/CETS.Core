using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Submission.Requests
{
    public class GradeSubmissionRequest
    {
        public Guid SubmissionID { get; set; }
        public decimal Score { get; set; }
        public string? Feedback { get; set; }
    }
}
