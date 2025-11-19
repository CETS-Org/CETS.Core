using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.IDN.IDN_Student.Responses
{
    public class StudentResponse
    {
        public Guid AccountId { get; set; }
        public string StudentCode { get; set; } = null!;

        public int StudentNumber { get; set; }

        public string? GuardianName { get; set; }

        public string? GuardianPhone { get; set; }

        public string? School { get; set; }

        public string? AcademicNote { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }

        public decimal PlacementTestGrade { get; set; }
    }
}
