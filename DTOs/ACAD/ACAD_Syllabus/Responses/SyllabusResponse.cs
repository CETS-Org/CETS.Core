using DTOs.ACAD.ACAD_SyllabusItem.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Syllabus.Responses
{
    public class SyllabusResponse
    {
        public Guid SyllabusID { get; set; }
        public Guid CourseID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public IEnumerable<SyllabusItemResponse> Items { get; set; } = new List<SyllabusItemResponse>();
    }
}
