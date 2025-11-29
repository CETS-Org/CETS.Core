using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.FAC.FAC_Room.Requests
{
    public class BookRoomSlotRequest
    {
        public Guid RoomId { get; set; }
        public Guid ClassId { get; set; }
        public Guid TeacherId { get; set; }
        public Guid CourseId { get; set; }
        public DateOnly Date { get; set; }
        public int SlotNumber { get; set; }
    }

}
