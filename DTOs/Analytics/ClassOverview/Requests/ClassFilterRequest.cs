using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Analytics.ClassOverview.Requests
{
    public class ClassFilterRequest
    {
        public Guid? CourseId { get; set; }
        public Guid? TeacherId { get; set; }
        public string? ClassStatus { get; set; } // "Active", "Completed", "Cancelled"
        public DateTime? StartDateFrom { get; set; }
        public DateTime? StartDateTo { get; set; }
        public bool? IsActive { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}



