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
        //Course info
        public Guid CourseId { get; set; }
        public string CourseCode { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public string? CourseImageUrl { get; set; }
        public string? Description { get; set; }
        public decimal StandardPrice { get; set; }
        public string? CategoryName { get; set; } 
        //---------//
        public Guid? InvoiceId { get; set; }
        public string? InvoiceStatus { get; set; }
        public string? PlanType { get; set; }
        public Guid ClassReservationId { get; set; }
    }
}
