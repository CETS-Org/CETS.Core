using DTOs.ACAD.ACAD_Course.Responses;
using DTOs.CORE.LookUp.Responses;
using DTOs.FIN.FIN_Invoice.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_ReservationItem.Responses
{
    public class ReservationItemResponse
    {
        public Guid Id { get; set; }
        public int? PaymentSequence { get; set; }
        public CourseResponse? Course { get; set; }
        public string? InvoiceStatus { get; set; }
        public string? PlanType { get; set; }
    }
}
