using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_AcademicRequest.Requests
{
    public class ProcessAcademicRequest
    {
        public Guid RequestID { get; set; }
        public Guid StatusID { get; set; }
        public string? Description { get; set; }
        public Guid StaffID { get; set; }
        public string? AttachmentUrl { get; set; }
        // For meeting reschedule - selected room by staff
        public Guid? SelectedRoomID { get; set; }
    }
}
