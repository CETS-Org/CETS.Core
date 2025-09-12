using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.IDN.IDN_Authentication
{
    public interface IAppUser
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public List<string> RoleNames { get; } 
    }
}
