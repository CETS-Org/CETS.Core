using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Class.Requests
{
    public class UpdateClassCompositeRequest
    {
        public string ClassName { get; set; } = string.Empty;
        public Guid? TeacherAssignmentID { get; set; }

        public Guid? SubTeacherAssignmentID { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int Capacity { get; set; }
        public Guid UpdatedBy { get; set; }

        // Danh sách StudentID mới của lớp
        public List<Guid> EnrollmentIds { get; set; } = new();
    }
}
