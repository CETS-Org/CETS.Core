using DTOs.ACAD.ACAD_ReservationItem.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_ClassReservation.Requests
{
    public class CreateClassReservationWithItemsRequest
    {
        [Required]
        public Guid StudentID { get; set; }

        public Guid? CoursePackageID { get; set; }
        public List<CreateReservationItemsRequest> Items { get; set; } = new List<CreateReservationItemsRequest>();

    }
}
