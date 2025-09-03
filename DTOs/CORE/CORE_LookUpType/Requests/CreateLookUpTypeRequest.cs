using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.CORE.LookUpType.Requests
{
    public class CreateLookUpTypeRequest
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
