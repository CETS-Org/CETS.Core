using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_SyllabusItem.Requests
{
    public class UpdateSyllabusItemRequest
    {
        public Guid SyllabusItemID { get; set; }
        public string? TopicTitle { get; set; }
        public int? EstimatedMinutes { get; set; }
        public bool? Required { get; set; }
        public string? Objectives { get; set; }
        public string? ContentSummary { get; set; }
        public string? PreReadingUrl { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
