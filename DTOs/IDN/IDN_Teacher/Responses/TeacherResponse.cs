using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.IDN.IDN_Teacher.Responses
{
    public class TeacherResponse
    {
        public Guid AccountId { get; set; }
        public string TeacherCode { get; set; } = null!;

        public int? YearsExperience { get; set; }

        public string? Bio { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
