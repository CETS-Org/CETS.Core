using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.IDN.IDN_Staff
{
    public class StaffDetailResponse
    {       
        public DateOnly? DateOfBirth { get; set; }

        public string? CID { get; set; }

        public string? Address { get; set; }

        public string? AvatarUrl { get; set; }
        public Guid? AccountStatusID { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }

        public string? StatusName { get; set; }
    }
}
