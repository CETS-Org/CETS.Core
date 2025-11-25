using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.FAC.FAC_Room.Responses
{
    public class SlotAvailabilityDto
    {
        public bool Available { get; set; }
        public string? Reason { get; set; }
        public Guid? ConflictBookingId { get; set; }
        public string? ConflictClassName { get; set; }
    }
}
