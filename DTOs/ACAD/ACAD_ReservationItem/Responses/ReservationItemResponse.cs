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
        public CourseDetailResponse? Course { get; set; }
        public InvoiceResponse? Invoice { get; set; }
        public string? PlanType { get; set; }
    }
}
