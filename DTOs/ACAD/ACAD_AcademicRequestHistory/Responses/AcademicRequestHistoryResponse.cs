using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_AcademicRequestHistory.Responses
{
    public class AcademicRequestHistoryResponse
    {
        public Guid Id { get; set; }
        public Guid RequestID { get; set; }
        public Guid StatusID { get; set; }
        public string? Description { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? AttachmentUrl { get; set; }
    }
}
