using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Assignment.Responses
{
    public class AssignmentWithSubmissionCountResponse
    {
        public Guid Id { get; set; }
        public Guid ClassMeetingId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? StoreUrl { get; set; }
        public DateTime? DueAt { get; set; }
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Tổng số submissions của assignment này
        /// </summary>
        public int SubmissionCount { get; set; }
        
      
        public Guid? SkillID { get; set; }
        
       
        public string? SkillName { get; set; }
    }
}

