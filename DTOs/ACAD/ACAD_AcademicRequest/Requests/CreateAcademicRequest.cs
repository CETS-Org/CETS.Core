using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_AcademicRequest.Requests
{
    public class CreateAcademicRequest
    {
        public Guid StudentID { get; set; }
        public Guid RequestTypeID { get; set; }
        public string Reason { get; set; } = null!;
        public Guid? FromClassID { get; set; }
        public Guid? ToClassID { get; set; }
        public DateOnly? EffectiveDate { get; set; }
        public string? AttachmentUrl { get; set; }
    }
}
