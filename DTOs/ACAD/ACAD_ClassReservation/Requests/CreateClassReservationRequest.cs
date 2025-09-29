using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.ACAD.ACAD_ClassReservation.Requests
{
    public class CreateClassReservationRequest
    {
        [Required]
        public Guid StudentID { get; set; }

        public Guid? CoursePackageID { get; set; }

        [Required]
        public DateTime ExpiresAt { get; set; }
    }
}
