using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_SyllabusItem.Requests
{
    public class CreateSyllabusItemRequest
    {
        public Guid SyllabusID { get; set; }
        public int SessionNumber { get; set; }
        public string TopicTitle { get; set; } = string.Empty;
        public int? TotalSlots { get; set; }
        public bool Required { get; set; } = true;
        public string? Objectives { get; set; }
        public string? ContentSummary { get; set; }
        public string? PreReadingUrl { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
