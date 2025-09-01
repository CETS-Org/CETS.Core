using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.IDN_TeacherCredential.Responses
{
    public class TeacherCredentialResponse
    {
        public Guid CredentialId { get; set; }
        public Guid TeacherId { get; set; }

        public Guid CredentialTypeId { get; set; }

        public string? PictureUrl { get; set; }

        public string Name { get; set; } = null!;

        public string Level { get; set; } = null!;

        public DateTime? UpdatedAt { get; set; }

        public Guid? UpdatedBy { get; set; }
    }
}
