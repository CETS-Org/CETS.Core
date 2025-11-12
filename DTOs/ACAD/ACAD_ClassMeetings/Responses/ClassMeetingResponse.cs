using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.ACAD.ACAD_Assignment.Responses;
using DTOs.ACAD.ACAD_SyllabusItem.Responses;

namespace DTOs.ACAD.ACAD_ClassMeetings.Responses
{
    public class ClassMeetingResponse
    {
        public Guid Id { get; set; }    
        public Guid ClassID { get; set; }

        public DateOnly Date { get; set; }

        public bool IsStudy { get; set; }

        public string? RoomID { get; set; }

        public string? OnlineMeetingUrl { get; set; }

        [StringLength(100)]
        public string? Passcode { get; set; }

        public string? RecordingUrl { get; set; }

        public string? ProgressNote { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

       
    }
}
