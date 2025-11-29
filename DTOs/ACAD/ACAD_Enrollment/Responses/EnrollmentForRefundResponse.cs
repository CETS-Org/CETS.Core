using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Enrollment.Responses
{
    public class EnrollmentForRefundResponse
    {
        public Guid EnrollmentId { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = null!;

        public Guid? InvoiceId { get; set; }
        public decimal? InvoiceTotal { get; set; }

        public decimal? FirstPaymentAmount { get; set; }
        public DateTime? FirstPaymentDate { get; set; }
        public string? FirstPaymentMethod { get; set; }

        public string CourseName { get; set; } = null!;
    }
}
