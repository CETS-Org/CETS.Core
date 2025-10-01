using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_ReservationItem.Requests
{
    public class CreateReservationItemsRequest
    {
        [Required]
        public Guid CourseID { get; set; }
        public Guid? InvoiceID { get; set; }
        public int? PaymentSequence { get; set; }
        public Guid? PlanTypeID { get; set; }
    }
}
