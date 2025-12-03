using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.FAC.FAC_Room.Responses
{
    public class RoomOptionDto
    {
        public Guid Id { get; set; }
        public string RoomCode { get; set; } = null!;
        public int Capacity { get; set; }
        public bool IsActive { get; set; }
    }
}
