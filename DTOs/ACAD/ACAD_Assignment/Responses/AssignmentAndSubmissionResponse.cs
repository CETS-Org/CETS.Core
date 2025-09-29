using DTOs.ACAD.ACAD_Submission.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Assignment.Responses
{
    public class AssignmentAndSubmissionResponse
    {
        public AssignmentResponse AssignmentResponse { get; set; }
        public SubmissionResponse? SubmissionResponse { get; set; }
    }

}
