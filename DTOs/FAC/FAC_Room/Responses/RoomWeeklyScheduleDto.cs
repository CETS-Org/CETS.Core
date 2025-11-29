using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.FAC.FAC_Room.Responses
{
    public class RoomWeeklyScheduleDto
    {
        public Guid RoomId { get; set; }
        public string RoomCode { get; set; } = null!;
        public string RoomStatus { get; set; } = null!;
        public string RoomTypeName { get; set; }


        public Dictionary<string, List<SlotScheduleDto>> Days { get; set; }
            = new Dictionary<string, List<SlotScheduleDto>>();
    }

    public class SlotScheduleDto
    {
        public int SlotNumber { get; set; }
        public bool IsBooked { get; set; }
        public Guid? BookingId { get; set; }
        public string? ClassName { get; set; }
        public string? CourseName { get; set; }
        public string? TeacherName { get; set; }
    }

}
