using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.FAC.FAC_Room.Responses
{
    public class RoomSlotInfoResponse
    {
        public RoomInfo Room { get; set; } = null!;
        public SlotInfo Slot { get; set; } = null!;
        public ClassInfo? CurrentClass { get; set; }
        public bool IsBooked { get; set; }

        public class RoomInfo
        {
            public Guid RoomId { get; set; }
            public string RoomCode { get; set; } = null!;
            public string RoomType { get; set; } = null!;
            public string Status { get; set; } = null!;
            public int Capacity { get; set; }
        }

        public class SlotInfo
        {
            public int SlotNumber { get; set; }
            public string Start { get; set; } = null!;
            public string End { get; set; } = null!;
            public DateOnly Date { get; set; }
            public string DayOfWeek { get; set; } = null!;
        }

        public class ClassInfo
        {
            public Guid MeetingId { get; set; }
            public string ClassName { get; set; } = null!;
            public string CourseName { get; set; } = null!;
            public string TeacherName { get; set; } = null!;
        }
    }

}
