using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_AcademicRequest.Responses
{
    public class SuspensionValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
        public bool RequiresDocument { get; set; }
        public int DurationDays { get; set; }
        public int SuspensionCountThisYear { get; set; }
    }
}

