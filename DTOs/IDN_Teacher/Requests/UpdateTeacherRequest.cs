using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.IDN_Teacher.Requests
{
    public class UpdateTeacherRequest
    {
        public string? TeacherCode { get; set; }
        public int? YearsExperience { get; set; }
        public string? Bio { get; set; }
    }
}
