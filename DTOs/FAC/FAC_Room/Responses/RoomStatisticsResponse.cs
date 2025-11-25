using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.FAC.FAC_Room.Responses
{
    public class RoomStatisticsResponse
    {
        public int TotalRooms { get; set; }
        public int ActiveRooms { get; set; }
        public int MaintenanceRooms { get; set; }
        public int UnavailableRooms { get; set; }
    }

}
