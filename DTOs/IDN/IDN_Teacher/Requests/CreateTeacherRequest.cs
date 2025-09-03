using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.IDN.IDN_Teacher.Requests
{
    public class CreateTeacherRequest
    {
        public Guid AccountId { get; set; }
        public int? YearsExperience { get; set; }
        public string? Bio { get; set; }
    }
}
