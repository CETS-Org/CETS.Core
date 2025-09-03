using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_SyllabusItem.Responses
{
    public class SyllabusItemResponse
    {
        public Guid Id { get; set; }
        public int SessionNumber { get; set; }
        public string TopicTitle { get; set; } = string.Empty;
        public int? EstimatedMinutes { get; set; }
        public bool Required { get; set; }
        public string? Objectives { get; set; }
        public string? ContentSummary { get; set; }
        public string? PreReadingUrl { get; set; }
    }
}
