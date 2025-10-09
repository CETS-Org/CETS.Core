using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Attendance.Requests
{
    public class BulkAttendanceRequest
    {
        [Required]
        public Guid ClassMeetingId { get; set; }

        [Required]
        public Guid TeacherId { get; set; }

        /// <summary>
        /// Danh sách ID của học sinh vắng mặt
        /// </summary>
        public List<Guid> AbsentStudentIds { get; set; } = new List<Guid>();

        /// <summary>
        /// Ghi chú chung cho tất cả điểm danh (optional)
        /// </summary>
        public string? Notes { get; set; }
    }
}


