using System;

namespace DTOs.ACAD.ACAD_Class.Responses
{
    public class StudentInClassResponse
    {
        public Guid Id { get; set; }
        public Guid EnrollmentId { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string JoinDate { get; set; } = string.Empty;
        public decimal AttendanceRate { get; set; }
        public decimal ProgressPercentage { get; set; }
        public decimal? FinalGrade { get; set; }
        public bool IsPass { get; set; }
    }
}

