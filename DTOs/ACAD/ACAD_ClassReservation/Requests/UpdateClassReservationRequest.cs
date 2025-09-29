using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_ClassReservation.Requests
{
    public class UpdateClassReservationRequest
    {
        public Guid Id { get; set; }
        public Guid? CoursePackageID { get; set; }
        public Guid? ReservationStatusID { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
