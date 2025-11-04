using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ACAD.ACAD_Class.Requests
{
    public class UpdateClassRequest
    {
        public Guid Id { get; set; }
        public Guid ClassStatusID { get; set; }
        public Guid? CourseFormatID { get; set; }
        public Guid? TeacherAssignmentID { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
