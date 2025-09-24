using DTOs.IDN.IDN_TeacherCredential.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.IDN.IDN_Teacher.Requests
{
    public class UpdateTeacherProfileRequest
    {
        public string? TeacherCode { get; set; }
        public int? YearsExperience { get; set; }
        public string? Bio { get; set; }
        public string? FullName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? CID { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }
        public List<UpdateTeacherCredentialRequest>? Credentials { get; set; }

    }
}
