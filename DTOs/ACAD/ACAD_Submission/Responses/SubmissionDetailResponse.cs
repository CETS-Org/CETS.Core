using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Submission.Responses
{
    public class SubmissionDetailResponse : SubmissionResponse
    {
        public string? StudentName { get; set; }
        public string? AssignmentTitle { get; set; }
    }
}
