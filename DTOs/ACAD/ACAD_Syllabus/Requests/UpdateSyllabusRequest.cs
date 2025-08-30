using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Syllabus.Requests
{
    public class UpdateSyllabusRequest
    {
        public Guid SyllabusID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
