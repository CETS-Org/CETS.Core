using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Submission.Requests
{
    public class SubmitAssignmentRequest
    {
        public Guid AssignmentID { get; set; }
        public Guid StudentID { get; set; }
        public string? FileUrl { get; set; }
        public string? Content { get; set; }
    }
}
