using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.IDN.IDN_Teacher.Responses
{
    public class TeacherAcademicDetailResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? Bio { get; set; }
        public double Rating { get; set; }
        public int TotalStudents { get; set; }
        public int TotalCourses { get; set; }
        public int? YearsExperience { get; set; }
    }
}
