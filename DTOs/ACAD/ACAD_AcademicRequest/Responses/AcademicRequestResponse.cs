using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_AcademicRequest.Responses
{
    public class AcademicRequestResponse
    {
        public Guid Id { get; set; }
        public Guid StudentID { get; set; }
        public string? StudentName { get; set; }
        public string? StudentEmail { get; set; }

        public Guid RequestTypeID { get; set; }
        public string? RequestTypeName { get; set; }

        public Guid AcademicRequestStatusID { get; set; }
        public string? StatusName { get; set; }

        public string Reason { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public Guid? FromClassID { get; set; }
        public string? FromClassName { get; set; }

        public Guid? ToClassID { get; set; }
        public string? ToClassName { get; set; }

        public DateOnly? EffectiveDate { get; set; }
        public string? AttachmentUrl { get; set; }

        public Guid? ProcessedBy { get; set; }
        public string? ProcessedByName { get; set; }
        public DateTime? ProcessedAt { get; set; }
    }
}
