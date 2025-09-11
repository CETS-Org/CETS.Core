using DTOs.IDN.IDN_Student.Responses;
using DTOs.IDN.IDN_Teacher.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.IDN.IDN_Account.Responses
{
    public class AccountDetailResponse : AccountResponse
    {
        public string? StatusName { get; set; }

        public List<string> RoleNames { get; set; } = new();

        public StudentResponse? StudentInfo { get; set; }

        public TeacherDetailResponse? TeacherInfo { get; set; }
    }
}
