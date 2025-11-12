using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Submission.Responses
{
    public class WeeklySubmissionPerformanceResponse
    {
        public int WeekNumber { get; set; }           
        public int TotalSubmissions { get; set; }      
        public int GradedSubmissions { get; set; }   
        public decimal TotalScore { get; set; }      
        public decimal AverageScore { get; set; }      
        public string PerformanceLevel { get; set; } = "N/A";
    }
}
