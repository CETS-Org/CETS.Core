using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.COM.COM_Email.Requests
{
    public class PostponedClassNotifyRequest
    {
        public string CourseName { get; set; } = null!;
        public DateTime PlannedStartDate { get; set; }

        public List<PostponedStudentItem> Students { get; set; } = new();
    }

    public class PostponedStudentItem
    {
        public Guid EnrollmentId { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public string StudentEmail { get; set; } = null!;
    }

    public class PostponedClassDecisionRequest
    {
        public Guid EnrollmentId { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public string StudentEmail { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public DateTime PlannedStartDate { get; set; }
        public string Decision { get; set; } = null!; // "refund" | "wait"
    }

}
