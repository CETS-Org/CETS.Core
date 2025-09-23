using System;

namespace DTOs.ACAD.ACAD_ClassReservation.Requests
{
    public class CreateClassReservationRequest
    {
        public Guid ClassID { get; set; }
        public Guid StudentID { get; set; }
        public DateTime ExpiresAt { get; set; }
        public Guid PaymentPlan { get; set; }
        public string? Notes { get; set; }
    }
}
