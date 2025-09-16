using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.IDN.IDN_TeacherCredential.Requests
{
    public class CreateTeacherCredentialRequest
    {
        public Guid CredentialTypeId { get; set; }
        public string? PictureUrl { get; set; }
        public string Name { get; set; } = null!;
        public string Level { get; set; } = null!;
    }
}
